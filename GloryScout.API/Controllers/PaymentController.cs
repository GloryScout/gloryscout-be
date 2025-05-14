//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Threading.Tasks;
//using System.Text.Json.Serialization;
//using GloryScout.API.Services;
//using GloryScout.Data.Models.Payment;

//namespace YourNamespace.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class PaymentController : ControllerBase
//    {
//        private readonly PaymobService _paymob;
//        private readonly IConfiguration _config;
//        private readonly ILogger<PaymentController> _logger;
//        private readonly IOrderService _orderService;

//        public PaymentController(PaymobService paymob, IConfiguration config, ILogger<PaymentController> logger, IOrderService orderService)
//        {
//            _paymob = paymob;
//            _config = config;
//            _logger = logger;
//            _orderService = orderService;
//        }

//        [HttpPost("pay")]
//        public async Task<IActionResult> Pay([FromBody] PaymentRequest request)
//        {
//            if (request == null || request.AmountCents <= 0)
//            {
//                return BadRequest(new { message = "قيمة المبلغ يجب أن تكون أكبر من صفر." });
//            }

//            try
//            {
//                var token = await _paymob.GetAuthTokenAsync();
//                var orderId = await _paymob.CreateOrderAsync(token, request.AmountCents);
//                var paymentKey = await _paymob.GetPaymentKeyAsync(
//                    token,
//                    request.AmountCents,
//                    orderId,
//                    request.BillingData
//                );

//                var iframeId = _config["Paymob:IframeId"];
//                var url = $"https://accept.paymob.com/api/acceptance/iframes/{iframeId}?payment_token={paymentKey}";
//                return Ok(new { url });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "خطأ أثناء تنفيذ عملية الدفع عبر Paymob.");
//                return StatusCode(500, new
//                {
//                    message = "حدث خطأ أثناء تنفيذ الدفع. برجاء المحاولة لاحقًا.",
//                    error = ex.Message
//                });
//            }
//        }

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

//        // PaymentRequest class for the payment request body
//        public class PaymentRequest
//        {
//            public int AmountCents { get; set; }
//            public BillingData BillingData { get; set; }
//        }

//        // BillingData class that holds customer information
//        public class BillingData
//        {
//            [JsonPropertyName("first_name")]
//            public string FirstName { get; set; }

//            [JsonPropertyName("last_name")]
//            public string LastName { get; set; }

//            [JsonPropertyName("phone_number")]
//            public string PhoneNumber { get; set; }

//            [JsonPropertyName("apartment")]
//            public string Apartment { get; set; }

//            [JsonPropertyName("email")]
//            public string Email { get; set; }

//            [JsonPropertyName("floor")]
//            public string Floor { get; set; }

//            [JsonPropertyName("street")]
//            public string Street { get; set; }

//            [JsonPropertyName("building")]
//            public string Building { get; set; }

//            [JsonPropertyName("shipping_method")]
//            public string ShippingMethod { get; set; }

//            [JsonPropertyName("postal_code")]
//            public string PostalCode { get; set; }

//            [JsonPropertyName("city")]
//            public string City { get; set; }

//            [JsonPropertyName("country")]
//            public string Country { get; set; }

//            [JsonPropertyName("state")]
//            public string State { get; set; }
//        }

//        // Paymob Callback response for when Paymob sends data after the payment process
//        public class ControllerPaymobCallbackResponse
//        {
//            public string PaymentStatus { get; set; }
//            public string OrderId { get; set; }
//            public string PaymentId { get; set; }
//        }
//    }
//}
