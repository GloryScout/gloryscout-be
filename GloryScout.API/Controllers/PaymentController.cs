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
        public async Task<IActionResult> Pay(int amountCents)
        {
            if (amountCents <= 0)
            {
                return BadRequest(new { message = "قيمة المبلغ يجب أن تكون أكبر من صفر." });
            }

            try
            {
                var token = await _paymob.GetAuthTokenAsync();
                var orderId = await _paymob.CreateOrderAsync(token, amountCents);
                var paymentKey = await _paymob.GetPaymentKeyAsync(token, amountCents, orderId);
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
    }
}
