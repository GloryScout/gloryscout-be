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
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Position { get; set; }
		public string DominantFoot { get; set; }
		public string PhoneNumber { get; set; }
		public string ProfileDescription { get; set; } 
		public int Weight { get; set; }
		public int Height { get; set; }
		public string CurrentTeam { get; set; }
		public Guid UserId { get; set; }

		// Navigation properties
		[ForeignKey("UserId")] public virtual User User { get; set; } = new User();
		public ICollection<Application> Applications { get; set; }
	}
}
