using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Domain.Dtos
{
	public class CreateScoutProfileDto
	{
		public string ClubName { get; set; }
		public string ProfileDescription { get; set; }
		public string ContactDetails { get; set; }
		public string Location { get; set; }
	}

	public class UpdateScoutProfileDto
	{
		public string ClubName { get; set; }
		public string ProfileDescription { get; set; }
		public string ContactDetails { get; set; }
		public string Location { get; set; }
	}
}
