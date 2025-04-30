using GloryScout.Data.Models.Followers;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace GloryScout.Data
{


	public sealed class User : IdentityUser<Guid>
	{
		public User()
		{
			VerificationCodes = new HashSet<VerificationCode>();
			Posts = new HashSet<Post>();
			Comments = new HashSet<Comment>();
			Likes = new HashSet<Like>();
			Followers = new HashSet<UserFollowings>();
			Following = new HashSet<UserFollowings>();
		}
		public string Nationality { get; set; } = "Egyptian";
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public string UserType { get; set; }
		public bool IsVerified { get; set; } = false;
		public string? ProfilePhoto { get; set; } // profile photo URL
		public string ProfileDescription { get; set; }

		// Navigation properties
		public ICollection<VerificationCode> VerificationCodes { get; set; }
		public ICollection<Post> Posts { get; set; }
		public ICollection<Comment> Comments { get; set; }
		public ICollection<Like> Likes { get; set; }
		public ICollection<UserFollowings> Followers { get; set; }
		public ICollection<UserFollowings> Following { get; set; }

	}
}
