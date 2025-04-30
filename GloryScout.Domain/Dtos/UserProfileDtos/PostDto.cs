using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Domain.Dtos.UserProfileDtos
{
    public class PostDto: IDtos
	{
		public Guid Id { get; set; }
		public string? Description { get; set; }
		public string? PosrUrl { get; set; }
	}
    
    
}
