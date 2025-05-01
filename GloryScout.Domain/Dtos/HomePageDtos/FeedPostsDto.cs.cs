using GloryScout.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Domain.Dtos.HomePageDtos
{
	public class FeedPostsDto : IDtos
	{
		public Guid Id { get; set; }
		public string Description { get; set; }
		public string PosrUrl { get; set; }
		public DateTime CreatedAt { get; set; }
		public Guid UserId { get; set; }
		public string Username { get; set; }
		public string UserProfilePicture { get; set; }

		// Added fields for post metrics
		public int LikesCount { get; set; }
		public int CommentsCount { get; set; }
		public bool IsLikedByCurrentUser { get; set; }
	}
}
