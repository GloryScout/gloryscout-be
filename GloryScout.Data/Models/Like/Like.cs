using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Data
{
	public class Like
	{
		public Guid Id { get; set; }
		public Guid PostId { get; set; }
		public Guid UserId { get; set; }
		public DateTime LikedAt { get; set; } = DateTime.Now;

		// Navigation properties
		[ForeignKey("PostId")] public virtual Post Post { get; set; } = new Post();
		[ForeignKey("UserId")] public virtual User User { get; set; } = new User();
	}
}
