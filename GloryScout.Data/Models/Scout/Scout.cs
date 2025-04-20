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
		public string? ClubName { get; set; }
		public string? ProfileDescription { get; set; } // will be in the player profile
		public string? Specialization { get; set; }
		public int? Experience { get; set; }
		public string? CurrentClubName { get; set; }
		public string? CoachingSpecialty { get; set; }
		public Guid UserId { get; set; }

		// Navigation properties
		[ForeignKey("UserId")] public virtual User User { get; set; } 
		public ICollection<Application> Applications { get; set; }
	}
}
