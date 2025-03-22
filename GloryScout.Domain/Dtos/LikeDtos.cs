using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Domain.Dtos
{
	public class CreateLikeDto
	{
		public Guid PostId { get; set; }
	}
	public class RemoveLikeDto
	{
		public Guid Id { get; set; }
	}
}
