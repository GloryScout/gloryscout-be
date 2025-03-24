using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Data
{
	public class Post
	{
		public Post()
		{
			Comments = new HashSet<Comment>();
			Likes = new HashSet<Like>();
		}
		public Guid Id { get; set; }
		public string Description { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public Guid UserId { get; set; }
		public string B2UrlKey { get; set; }

		// Navigation properties
		[ForeignKey("UserId")] public virtual User User { get; set; } 
		public virtual ICollection<Comment> Comments { get; set; }
		public virtual ICollection<Like> Likes { get; set; }
	}
}
