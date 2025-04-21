using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Domain.Dtos.IdentityDtos
{
	public class VerifyResetCodeDto
	{
		[EmailAddress]
		[Required]
		public string Email { get; set; }
		[Required,StringLength(10)]
		public string Code { get; set; }
	}
}
