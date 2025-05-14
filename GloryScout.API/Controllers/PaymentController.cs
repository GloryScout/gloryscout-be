using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly PaymobService _paymob;
        private readonly IConfiguration _config;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(PaymobService paymob, IConfiguration config, ILogger<PaymentController> logger)
        {
            _paymob = paymob;
            _config = config;
            _logger = logger;
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
				var token = await _paymob.GetAuthTokenAsync();
				var orderId = await _paymob.CreateOrderAsync(token, request.AmountCents);
				var paymentKey = await _paymob.GetPaymentKeyAsync(
					token,
					request.AmountCents,
					orderId,
					request.BillingData
				);

				var iframeId = _config["Paymob:IframeId"];
				var url = $"https://accept.paymob.com/api/acceptance/iframes/{iframeId}?payment_token={paymentKey}";
				return Ok(new { url });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "خطأ أثناء تنفيذ عملية الدفع عبر Paymob.");
				return StatusCode(500, new
				{
					message = "حدث خطأ أثناء تنفيذ الدفع. برجاء المحاولة لاحقًا.",
					error = ex.Message
				});
			}
		}



		[HttpPost]
		public async Task<IActionResult> ReceiveCallback([FromBody] PaymobCallbackData data)
		{
			// TODO: Validate HMAC signature here for security

			// Process the received data
			// For example, update order status in your database

			return Ok(); // Respond with 200 OK to acknowledge receipt
		}






		public class PaymentRequest
		{
			public int AmountCents { get; set; }
			public BillingData BillingData { get; set; }
		}

		public class BillingData
		{
			[JsonPropertyName("first_name")] // Snake case for Paymob API
			public string FirstName { get; set; }

			[JsonPropertyName("last_name")]
			public string LastName { get; set; }

			[JsonPropertyName("phone_number")]
			public string PhoneNumber { get; set; }

			// Other properties (keep snake_case):
			[JsonPropertyName("apartment")]
			public string Apartment { get; set; }

			[JsonPropertyName("email")]
			public string Email { get; set; }

			[JsonPropertyName("floor")]
			public string Floor { get; set; }

			[JsonPropertyName("street")]
			public string Street { get; set; }

			[JsonPropertyName("building")]
			public string Building { get; set; }

			[JsonPropertyName("shipping_method")]
			public string ShippingMethod { get; set; }

			[JsonPropertyName("postal_code")]
			public string PostalCode { get; set; }

			[JsonPropertyName("city")]
			public string City { get; set; }

			[JsonPropertyName("country")]
			public string Country { get; set; }

			[JsonPropertyName("state")]
			public string State { get; set; }
		}





		public class PaymobCallbackData
		{
			[JsonPropertyName("transaction_id")]
			public string TransactionId { get; set; }

			[JsonPropertyName("issuer")]
			public string Issuer { get; set; }

			[JsonPropertyName("amount")]
			public decimal Amount { get; set; }

			[JsonPropertyName("disbursement_status")]
			public string DisbursementStatus { get; set; }

			[JsonPropertyName("status_code")]
			public string StatusCode { get; set; }

			[JsonPropertyName("status_description")]
			public string StatusDescription { get; set; }

			[JsonPropertyName("created_at")]
			public string CreatedAt { get; set; }

			[JsonPropertyName("updated_at")]
			public string UpdatedAt { get; set; }

			// Include other fields as needed based on Paymob's callback payload
		}

	}
}
