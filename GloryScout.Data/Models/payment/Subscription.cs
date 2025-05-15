using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Data.Models.payment
{
    public class Subscription
    {

		[Key]
		public int Id { get; set; }

		// Foreign key for the user who paid
		public Guid UserId { get; set; }

		// Navigation to the paying user
		[ForeignKey("UserId")]
		public virtual User User { get; set; }

		// Foreign key for the user whose data is accessed
		public Guid RequestedUserId { get; set; }

		// Navigation to the requested user
		[ForeignKey("RequestedUserId")]
		public virtual User RequestedUser { get; set; }
		public int TransactionId { get; set; }
		public long OrderId { get; set; }
		public string Status { get; set; }
		public bool Success { get; set; }
		public int AmountCents { get; set; }
		public bool Pending { get; set; }
		public DateTime CreatedAt { get; set; }
		public bool IsCancel { get; set; }
		public bool IsReturned { get; set; }
		public string PaymentStatus { get; set; }
	}
}
