using GloryScout.Data.Models.Payment;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net;
using System.Threading.Tasks;
using  GloryScout.API.Controllers;
using GloryScout.Data.Models.payment;
using static GloryScout.Controllers.PaymentController;
using GloryScout.Controllers;

namespace gloryscout.api.services
{
    public class paymobservice
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public paymobservice(HttpClient http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        public async Task<string> GetAuthTokenAsync()
        {
            var response = await _http.PostAsJsonAsync("https://accept.paymob.com/api/auth/tokens", new
            {
                api_key = _config["Paymob:ApiKey"]
            });

            if (!response.IsSuccessStatusCode)
            {
                var errorcontent = await response.Content.ReadAsStringAsync();
                throw new Exception($"failed to retrieve auth token: {response.StatusCode} - {errorcontent}");
            }

            var result = await response.Content.ReadFromJsonAsync<AuthTokenResponse>();
            return result?.Token ?? throw new Exception("no token received from paymob");
        }

		public async Task<int> CreateOrderAsync(string token, int amountCents, string merchantOrderId)
		{
			var response = await _http.PostAsJsonAsync("https://accept.paymob.com/api/ecommerce/orders", new
			{
				auth_token = token,
				delivery_needed = false,
				amount_cents = amountCents,
				currency = "EGP",
				merchant_order_id = merchantOrderId // Ensure uniqueness
			});

			if (!response.IsSuccessStatusCode)
			{
				var errorContent = await response.Content.ReadAsStringAsync();
				throw new Exception($"Failed to create order: {errorContent}");
			}

			var result = await response.Content.ReadFromJsonAsync<OrderResponse>();
			return result?.Id ?? throw new Exception("No order ID received.");
		}

        public async Task<string> GetPaymentKeyAsync(string token, int amountcents, int orderid, BillingData billingdata )
        {
            var response = await _http.PostAsJsonAsync(
                "https://accept.paymob.com/api/acceptance/payment_keys",
                new
                {
                    auth_token = token,
                    amount_cents = amountcents,
                    expiration = 3600,
                    order_id = orderid,
                    billing_data = billingdata,
                    currency = "EGP",
                    integration_id = int.Parse(_config["Paymob:IntegrationId"])
                });

            if (!response.IsSuccessStatusCode)
            {
                var errorcontent = await response.Content.ReadAsStringAsync();
                throw new Exception($"paymob api error: {response.StatusCode} - {errorcontent}");
            }

            var result = await response.Content.ReadFromJsonAsync<PaymentKeyResponse>();
            return result?.Token ?? throw new Exception("no token received from paymob");
        }
    }
}
