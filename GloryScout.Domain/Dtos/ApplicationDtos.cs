using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Domain.Dtos
{
	public class CreateApplicationDto
	{
		public Guid PlayerId { get; set; }
		public string Content { get; set; }
	}

	public class UpdateApplicationContentDto
	{
		public Guid ApplicationId { get; set; }
		public string Content { get; set; }
	}
}
