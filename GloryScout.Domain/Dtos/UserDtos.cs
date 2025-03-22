using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Domain.Dtos
{
	public class RegisterUserDto
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public string ConfirmPassword { get; set; }
		public string Nationality { get; set; }
		public string UserType { get; set; }
	}

	public class LoginUserDto
	{
		public string Email { get; set; }
		public string Password { get; set; }
	}

	public class VerifyUserDto
	{
		public string Email { get; set; }
		public string Code { get; set; }
	}
}
