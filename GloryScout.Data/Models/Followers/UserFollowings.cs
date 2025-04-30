using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Data.Models.Followers
{
    public class UserFollowings
    {
		public Guid FollowerId { get; set; } // The user who is following(me basically)
		[ForeignKey("FollowerId")] public User Follower { get; set; }

		public Guid FolloweeId { get; set; } // The user being followed
		[ForeignKey("FolloweeId")] public User Followee { get; set; }

		public DateTime FollowedAt { get; set; } = DateTime.Now;

	}

	/*
		FollowerId: A foreign key referencing the User who is following someone.
		FolloweeId: A foreign key referencing the User being followed.
		FollowedAt: A timestamp to record when the follow occurred, useful for sorting or analytics.
		Navigation properties (Follower and Followee) link back to the User model.
	 
	 */
}
