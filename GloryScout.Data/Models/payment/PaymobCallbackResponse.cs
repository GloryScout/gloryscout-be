using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Data.Models.Payment
{
    public class PaymobCallbackResponse
    {
        public int Id { get; set; }
        public string PaymentStatus { get; set; }
        public string OrderId { get; set; }
        public string PaymentId { get; set; }
    }
}


