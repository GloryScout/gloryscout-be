using GloryScout.Data.Models.payment;

public class PaymobService
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;

    public PaymobService(HttpClient http, IConfiguration config)
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

        var result = await response.Content.ReadFromJsonAsync<AuthTokenResponse>();
        return result.Token;
    }

    public async Task<int> CreateOrderAsync(string token, int amountCents)
    {
        var response = await _http.PostAsJsonAsync("https://accept.paymob.com/api/ecommerce/orders", new
        {
            auth_token = token,
            delivery_needed = false,
            amount_cents = amountCents.ToString(),
            currency = "EGP",
            items = new object[] { }
        });

        var result = await response.Content.ReadFromJsonAsync<OrderResponse>();
        return result.Id;
    }

    public async Task<string> GetPaymentKeyAsync(string token, int amountCents, int orderId)
    {
        var response = await _http.PostAsJsonAsync("https://accept.paymob.com/api/acceptance/payment_keys", new
        {
            auth_token = token,
            amount_cents = amountCents.ToString(),
            expiration = 3600,
            order_id = orderId,
            billing_data = new
            {
                apartment = "803",
                email = "user@example.com",
                floor = "42",
                first_name = "Mohamed",
                street = "Example Street",
                building = "8028",
                phone_number = "+201000000000",
                shipping_method = "PKG",
                postal_code = "01898",
                city = "Cairo",
                country = "EG",
                last_name = "Sayed",
                state = "CA"
            },
            currency = "EGP",
            integration_id = int.Parse(_config["Paymob:IntegrationId"])
        });

        var result = await response.Content.ReadFromJsonAsync<PaymentKeyResponse>();
        return result.Token;
    }
}
