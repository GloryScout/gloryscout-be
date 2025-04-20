using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Domain.Dtos.IdentityDtos
{
	public class CoachRegisterDto
	{
		public string Username { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string Specialization { get; set; }
		public int Experience { get; set; }
		public string CurrentClubName { get; set; }
		public string? CoachingSpecialty { get; set; }
		public string PhoneNumber { get; set; }
	}
}
