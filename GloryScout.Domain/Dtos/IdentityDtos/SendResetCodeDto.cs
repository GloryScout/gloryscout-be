using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Domain.Dtos.IdentityDtos
{
	public class SendResetCodeDto
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}
}
