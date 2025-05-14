//using GloryScout.Data.Models.payment;
//using GloryScout.Data.Models.Payment;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Net.Http;
//using System.Net.Http.Json;
//using System.Threading.Tasks;
//using static YourNamespace.Controllers.PaymentController;

//namespace GloryScout.API.Services
//{
//    public class PaymobService
//    {
//        private readonly HttpClient _http;
//        private readonly IConfiguration _config;

//        public PaymobService(HttpClient http, IConfiguration config)
//        {
//            _http = http;
//            _config = config;
//        }

//        public async Task<string> GetAuthTokenAsync()
//        {
//            var response = await _http.PostAsJsonAsync("https://accept.paymob.com/api/auth/tokens", new
//            {
//                api_key = _config["Paymob:ApiKey"]
//            });

//            if (!response.IsSuccessStatusCode)
//            {
//                var errorContent = await response.Content.ReadAsStringAsync();
//                throw new Exception($"Failed to retrieve auth token: {response.StatusCode} - {errorContent}");
//            }

//            var result = await response.Content.ReadFromJsonAsync<AuthTokenResponse>();
//            return result?.Token ?? throw new Exception("No token received from Paymob");
//        }

//        public async Task<int> CreateOrderAsync(string token, int amountCents)
//        {
//            var response = await _http.PostAsJsonAsync("https://accept.paymob.com/api/ecommerce/orders", new
//            {
//                auth_token = token,
//                delivery_needed = false,
//                amount_cents = amountCents.ToString(),
//                currency = "EGP",
//                items = new object[] { }
//            });

//            if (!response.IsSuccessStatusCode)
//            {
//                var errorContent = await response.Content.ReadAsStringAsync();
//                throw new Exception($"Failed to create order: {response.StatusCode} - {errorContent}");
//            }

//            var result = await response.Content.ReadFromJsonAsync<OrderResponse>();
//            return result?.Id ?? throw new Exception("No order ID returned from Paymob");
//        }

//        public async Task<string> GetPaymentKeyAsync(string token, int amountCents, int orderId, BillingData billingData)
//        {
//            var response = await _http.PostAsJsonAsync(
//                "https://accept.paymob.com/api/acceptance/payment_keys",
//                new
//                {
//                    auth_token = token,
//                    amount_cents = amountCents,
//                    expiration = 3600,
//                    order_id = orderId,
//                    billing_data = billingData,
//                    currency = "EGP",
//                    integration_id = int.Parse(_config["Paymob:IntegrationId"])
//                });

//            if (!response.IsSuccessStatusCode)
//            {
//                var errorContent = await response.Content.ReadAsStringAsync();
//                throw new Exception($"Paymob API Error: {response.StatusCode} - {errorContent}");
//            }

//            var result = await response.Content.ReadFromJsonAsync<PaymentKeyResponse>();
//            return result?.Token ?? throw new Exception("No token received from Paymob");
//        }
//    }
//}
