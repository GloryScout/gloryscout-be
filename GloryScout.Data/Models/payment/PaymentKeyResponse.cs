using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GloryScout.Data.Models.Payment
{
    public class PaymentKeyResponse
    {
        [JsonPropertyName("token")] // Match Paymob's lowercase property name
        public string Token { get; set; }
    }
}
