using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Domain.Dtos
{
	public class CreatePostDto
	{
		public string Description { get; set; }
		public IFormFile Image { get; set; }
	}

	public class UpdatePostDto
	{
		public Guid Id { get; set; }
		public string Description { get; set; }
	}
}
