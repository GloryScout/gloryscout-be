using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Domain.Dtos.UserProfileDtos
{
    public class OtherUserProfile
    {
		public OtherUserProfile()
		{
			
		}
		public Guid Id { get; set; }
		public string Username { get; set; }
		public string? ProfilePhoto { get; set; }
		public string? ProfileDescription { get; set; }

		public ICollection<PostDto> Posts { get; set; }

		public int FollowersCount { get; set; }
		public int FollowingCount { get; set; }
		public bool IsFollowing { get; set; } 
	}
}
