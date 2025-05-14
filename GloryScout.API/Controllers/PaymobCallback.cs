
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using GloryScout.Data;
//using GloryScout.Data.Models;
//using System.Threading.Tasks;
//using GloryScout.Data.Models.Payment;

//namespace GloryScout.API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class PaymentController : ControllerBase
//    {
//        private readonly ILogger<PaymentController> _logger;
//        private readonly IOrderService _orderService;

//        public PaymentController(ILogger<PaymentController> logger, IOrderService orderService)
//        {
//            _logger = logger;
//            _orderService = orderService;
//        }

//        [HttpPost("payment-callback")]
//        public async Task<IActionResult> PaymentCallback([FromBody] PaymobCallbackResponse callbackResponse)
//        {
//            if (callbackResponse == null || string.IsNullOrEmpty(callbackResponse.OrderId))
//            {
//                _logger.LogWarning("Received invalid callback data: Missing OrderId.");
//                return BadRequest("Invalid callback data.");
//            }

//            var order = await _orderService.GetOrderByIdAsync(callbackResponse.OrderId);
//            if (order == null)
//            {
//                _logger.LogWarning($"Order not found for OrderId: {callbackResponse.OrderId}");
//                return NotFound("Order not found.");
//            }

//            if (callbackResponse.PaymentStatus == "paid")
//            {
//                if (!order.IsPayed)
//                {
//                    order.IsPayed = true;
//                    await _orderService.UpdateOrderAsync(order);

//                    _logger.LogInformation($"Payment for Order ID: {order.Id} has been successfully processed.");
//                    return Ok("Payment status updated.");
//                }
//                else
//                {
//                    _logger.LogInformation($"Payment for Order ID: {order.Id} was already processed.");
//                    return Ok("Payment already processed.");
//                }
//            }

//            _logger.LogWarning($"Payment for Order ID: {callbackResponse.OrderId} failed. Payment Status: {callbackResponse.PaymentStatus}");
//            return BadRequest("Payment was not successful.");
//        }
//    }
//}