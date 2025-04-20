using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Domain.Dtos.IdentityDtos
{
	public class PlayerRegisterDto
	{
		public string Username { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public int Age { get; set; }
		public float Height { get; set; }
		public float Weight { get; set; }
		public string Position { get; set; }
		public string PhoneNumber { get; set; }
	}
}
