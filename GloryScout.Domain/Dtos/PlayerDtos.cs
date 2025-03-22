using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Domain.Dtos
{
	public class CreatePlayerProfileDto
	{
		public string Name { get; set; }
		public string Position { get; set; }
		public string DominantFoot { get; set; }
		public string PhoneNumber { get; set; }
		public string ProfileDescription { get; set; }
		public int Weight { get; set; }
		public int Height { get; set; }
		public string CurrentTeam { get; set; }
	}

	public class UpdatePlayerProfileDto
	{
		public string Name { get; set; }
		public string Position { get; set; }
		public string DominantFoot { get; set; }
		public string PhoneNumber { get; set; }
		public string ProfileDescription { get; set; }
		public int Weight { get; set; }
		public int Height { get; set; }
		public string CurrentTeam { get; set; }
	}
}
