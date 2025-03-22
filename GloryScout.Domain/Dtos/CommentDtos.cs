using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Domain.Dtos
{
	public class CreateCommentDto
	{
		public Guid PostId { get; set; }
		public string CommentedText { get; set; }
	}

	public class UpdateCommentDto
	{
		public Guid Id { get; set; }
		public string CommentedText { get; set; }
	}
}
