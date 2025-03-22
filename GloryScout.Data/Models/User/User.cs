using Microsoft.AspNetCore.Identity;

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
		}
		public string Nationality { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public string UserType { get; set; }
		public bool IsVerified { get; set; } = false;

		// Navigation properties
		public  ICollection<VerificationCode> VerificationCodes { get; set; }
		public  ICollection<Post> Posts { get; set; }
		public  ICollection<Comment> Comments { get; set; }
		public  ICollection<Like> Likes { get; set; }
	}
}
