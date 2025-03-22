using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Data
{
	public class VerificationCode
	{
		public Guid Id { get; set; }
		public bool IsUsed { get; set; } = false;
		public DateTime ExpiresAt { get; set; } = DateTime.Now.AddMinutes(10);
		public string Code { get; set; }
		public Guid UserId { get; set; }
		public string UserEmail { get; set; }// Why use UserEmail when VerificationCode is connected to the user by UserId?

        // Navigation properties
        [ForeignKey("UserId")] public virtual User User { get; set; } /*= new User();*/// Why have you used new User()? ,and the same thing for post,.....
	}
}
