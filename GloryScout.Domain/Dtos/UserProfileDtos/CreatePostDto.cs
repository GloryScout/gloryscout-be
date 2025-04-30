using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Domain.Dtos.UserProfileDtos
{
    public class CreatePostDto
    {
		public Guid UserId { get; set; }
		public string? Description { get; set; }
	}
}
