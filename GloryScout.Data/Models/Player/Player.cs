using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Data
{
	public class Player
	{
		public Player()
		{
			Applications = new HashSet<Application>();
		}
		public Guid Id { get; set; }
		public int? Age { get; set; } 
		public string? Position { get; set; }
		public string? DominantFoot { get; set; }
		public float? Weight { get; set; } 
		public float? Height { get; set; } 
		public string? CurrentTeam { get; set; }
		public string? ProfileDescription { get; set; } 
		public Guid UserId { get; set; }

		// Navigation properties
		[ForeignKey("UserId")] public virtual User User { get; set; } 
		public ICollection<Application> Applications { get; set; }
	}
}
