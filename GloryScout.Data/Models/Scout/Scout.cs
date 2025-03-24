using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Data
{
	public class Scout
	{
		public Scout()
		{
			Applications = new HashSet<Application>();
		}
		public Guid Id { get; set; }
		public string ClubName { get; set; }
		public string ProfileDescription { get; set; }
		public string ContactDetails { get; set; }
		public string Location { get; set; }
		public Guid UserId { get; set; }

		// Navigation properties
		[ForeignKey("UserId")] public virtual User User { get; set; } 
		public ICollection<Application> Applications { get; set; }
	}
}
