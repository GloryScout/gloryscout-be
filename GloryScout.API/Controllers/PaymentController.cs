using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using GloryScout.API.Services;
using GloryScout.Data.Models;
using GloryScout.Data.Models.Payment;
using GloryScout.API.Services;
using gloryscout.api.services;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Diagnostics;
using System.Text.Json;
using GloryScout.Data.Models.payment;


namespace GloryScout.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class PaymentController : ControllerBase
	{
		private readonly paymobservice _paymob;
		private readonly IConfiguration _config;
		private readonly ILogger<PaymentController> _logger;
		private readonly AppDbContext _dbContext;

		public PaymentController(
			paymobservice paymob,
			IConfiguration config,
			ILogger<PaymentController> logger,
			AppDbContext dbContext)
		{
			_paymob = paymob;
			_config = config;
			_logger = logger;
			_dbContext = dbContext;
		}




		[HttpPost("pay")]
		public async Task<IActionResult> Pay([FromBody] PaymentRequest request)
		{
			if (request == null || request.AmountCents <= 0)
			{
				return BadRequest(new { message = "قيمة المبلغ يجب أن تكون أكبر من صفر." });
			}

			try
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (string.IsNullOrEmpty(userId))
				{
					return Unauthorized(new { message = "المستخدم غير مصرح له." });
				}

				var user = await _dbContext.Users.FindAsync(Guid.Parse(userId));
				if (user == null)
				{
					return NotFound(new { message = "المستخدم غير موجود." });
				}

				// Check for existing pending subscription
				var existingSubscription = await _dbContext.Subscriptions
					.FirstOrDefaultAsync(s => s.UserId == Guid.Parse(userId) && s.RequestedUserId == request.RequestedUserId);

				if (existingSubscription != null)
				{
					return BadRequest("لديك طلب دفع معلق بالفعل.");
				}

				// Generate unique merchant_order_id
				var merchantOrderId = $"{userId}_{DateTime.UtcNow.Ticks}";

				// Create Paymob order
				var token = await _paymob.GetAuthTokenAsync();
				var orderId = await _paymob.CreateOrderAsync(token, request.AmountCents, merchantOrderId);

				// Save subscription to database
				var subscription = new Subscription
				{
					OrderId = orderId,  // Ensure OrderId is long in your model
					UserId = Guid.Parse(userId),
					RequestedUserId = request.RequestedUserId,
					Status = "Pending",
					Success = false,
					AmountCents = request.AmountCents,
					Pending = true,
					CreatedAt = DateTime.UtcNow,
					IsCancel = false,
					IsReturned = false,
					PaymentStatus = "Pending"
				};

				await _dbContext.Subscriptions.AddAsync(subscription);
				await _dbContext.SaveChangesAsync();

				// Get payment key
				var billingData = new BillingData
				{
					FirstName = user.UserName,
					LastName = "none",
					Email = user.Email,
					PhoneNumber = user.PhoneNumber,
					Apartment = "first apartment",
					Floor = "secod",
					Street = "adjbda",
					Building = "sfhsd",
					ShippingMethod = "none",
					PostalCode = "525225",
					City = "egypt",
					Country = "egypt",
					State = "cairo"
				};

				var paymentKey = await _paymob.GetPaymentKeyAsync(
					token,
					request.AmountCents,
					orderId,
					billingData
				);

				var iframeId = _config["Paymob:IframeId"];
				var url = $"https://accept.paymob.com/api/acceptance/iframes/{iframeId}?payment_token={paymentKey}";
				return Ok(new { url });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "خطأ أثناء تنفيذ عملية الدفع عبر Paymوب.");
				return StatusCode(500, new
				{
					message = "حدث خطأ أثناء تنفيذ الدفع. برجاء المحاولة لاحقًا.",
					error = ex.Message
				});
			}
		}

		[HttpPost("paymob/transaction-callback")]
		[AllowAnonymous]
		public async Task<IActionResult> PaymobTransactionCallback()
		{
			try
			{
				Request.EnableBuffering();
				Request.Body.Position = 0;

				using var reader = new StreamReader(Request.Body, leaveOpen: true);
				var rawJson = await reader.ReadToEndAsync();
				Request.Body.Position = 0; // Reset for model binding

				if (string.IsNullOrEmpty(rawJson))
				{
					_logger.LogError("Received empty callback payload.");
					return BadRequest("Empty payload.");
				}

				_logger.LogInformation("Raw Paymob callback: {RawJson}", rawJson);

				// Deserialize callback data
				var callbackData = JsonConvert.DeserializeObject<PaymobCallbackDto>(rawJson);
				if (callbackData?.Obj?.Order == null)
				{
					_logger.LogError("Invalid callback structure.");
					return BadRequest("Invalid callback data.");
				}

				// Extract order ID
				var orderId = callbackData.Obj.Order.OrderId;
				var subscription = await _dbContext.Subscriptions
					.FirstOrDefaultAsync(s => s.OrderId == orderId);

				if (subscription == null)
				{
					_logger.LogError("Subscription not found for OrderId: {OrderId}", orderId);
					return BadRequest("Subscription not found.");
				}

				// Update subscription from callback data
				subscription.TransactionId = callbackData.Obj.TransactionId;
				subscription.Success = callbackData.Obj.Success;
				subscription.Pending = callbackData.Obj.Pending;
				subscription.AmountCents = callbackData.Obj.AmountCents;

				// Parse datetime (ISO 8601 format)
				if (DateTimeOffset.TryParse(callbackData.Obj.CreatedAt, out var createdAt))
				{
					subscription.CreatedAt = createdAt.UtcDateTime;
				}
				else
				{
					_logger.LogWarning("Invalid CreatedAt format: {CreatedAt}", callbackData.Obj.CreatedAt);
				}

				// Map payment status
				subscription.PaymentStatus = callbackData.Obj.Order.PaymentStatus switch
				{
					"PAID" => "Paid",
					"DECLINED" => "Declined",
					_ => "Pending"
				};

				subscription.IsCancel = callbackData.Obj.Order.IsCanceled;
				subscription.IsReturned = callbackData.Obj.Order.IsReturned;

				await _dbContext.SaveChangesAsync();

				_logger.LogInformation("Subscription {OrderId} updated to status: {pending}", orderId, subscription.Pending);
				return Ok();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to process Paymob callback.");
				return StatusCode(500);
			}
		}


		[HttpGet("check-access/{requestedUserId}")]
		public async Task<IActionResult> CheckAccess(Guid requestedUserId)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrEmpty(userId))
			{
				return Unauthorized(new { message = "المستخدم غير مصرح له." });
			}

			// Check for a valid subscription
			var validSubscription = await _dbContext.Subscriptions
				.Where(s => s.UserId == Guid.Parse(userId)
					&& s.RequestedUserId == requestedUserId
					&& s.Success
					&& s.Pending==false)
				.OrderByDescending(s => s.CreatedAt)
				.FirstOrDefaultAsync();

			if (validSubscription == null)
			{
				return BadRequest(new { message = "User didn't pay to view this player's data." });
			}

			// Fetch requested user's data
			var requestedUser = await _dbContext.Users
				.AsNoTracking()
				.FirstOrDefaultAsync(u => u.Id == requestedUserId);

			if (requestedUser == null)
			{
				return NotFound(new { message = "Requested user not found." });
			}

			return Ok(new
			{
				PhoneNumber = requestedUser.PhoneNumber,
				Email = requestedUser.Email
			});
		}




		//        [HttpPost("callback")]
		//        public async Task<IActionResult> PaymentCallback([FromBody] ControllerPaymobCallbackResponse callbackResponse)
		//        {
		//            if (callbackResponse == null)
		//            {
		//                return BadRequest("Invalid callback data.");
		//            }

		//            if (string.IsNullOrEmpty(callbackResponse.PaymentStatus) || callbackResponse.PaymentStatus != "paid")
		//            {
		//                return BadRequest("Payment not successful or invalid status.");
		//            }

		//            var order = await _orderService.GetOrderByIdAsync(callbackResponse.OrderId);
		//            if (order == null)
		//            {
		//                return NotFound("Order not found.");
		//            }

		//            if (order.IsPayed)
		//            {
		//                return Ok("Payment already processed.");
		//            }

		//            order.IsPayed = true; // تحديث حالة الدفع
		//            await _orderService.UpdateOrderAsync(order); // تحديث الطلب في قاعدة البيانات

		//            return Ok("Payment status updated successfully.");
		//        }

		// PaymentRequest class for the payment request body
		public class PaymentRequest
		{
			public int AmountCents { get; set; }
			public Guid RequestedUserId { get; set; }
		}

		// BillingData class that holds customer information
		public class BillingData
		{
			[JsonPropertyName("first_name")]
			public string FirstName { get; set; }

			[JsonPropertyName("last_name")]
			public string LastName { get; set; }

			[JsonPropertyName("phone_number")]
			public string PhoneNumber { get; set; }

			[JsonPropertyName("email")]
			public string Email { get; set; }

			[JsonPropertyName("apartment")]
			public string Apartment { get; set; } = "first apartment";

			[JsonPropertyName("floor")]
			public string Floor { get; set; } = "second";

			[JsonPropertyName("street")]
			public string Street { get; set; } = "";

			[JsonPropertyName("building")]
			public string Building { get; set; } = "";

			[JsonPropertyName("shipping_method")]
			public string ShippingMethod { get; set; } = "";

			[JsonPropertyName("postal_code")]
			public string PostalCode { get; set; } = "";

			[JsonPropertyName("city")]
			public string City { get; set; } = "";

			[JsonPropertyName("country")]
			public string Country { get; set; } = "";

			[JsonPropertyName("state")]
			public string State { get; set; } = "";
		}

		//        // Paymob Callback response for when Paymob sends data after the payment process
		//        public class ControllerPaymobCallbackResponse
		//        {
		//            public string PaymentStatus { get; set; }
		//            public string OrderId { get; set; }
		//            public string PaymentId { get; set; }
		//        }
		#region callback Dto


		public class PaymobCallbackDto
		{
			[JsonProperty("obj")]
			public CallbackObject Obj { get; set; }
		}

		public class CallbackObject
		{
			[JsonProperty("order")]
			public OrderData Order { get; set; }

			[JsonProperty("id")]
			public int TransactionId { get; set; }

			[JsonProperty("success")]
			public bool Success { get; set; }

			[JsonProperty("pending")]
			public bool Pending { get; set; }

			[JsonProperty("amount_cents")]
			public int AmountCents { get; set; }

			[JsonProperty("created_at")]
			public string CreatedAt { get; set; }
		}

		public class OrderData
		{
			[JsonProperty("id")]
			public long OrderId { get; set; }

			[JsonProperty("is_canceled")]
			public bool IsCanceled { get; set; }

			[JsonProperty("is_returned")]
			public bool IsReturned { get; set; }

			[JsonProperty("status")]
			public string PaymentStatus { get; set; }
		}

		#endregion

	}
}

