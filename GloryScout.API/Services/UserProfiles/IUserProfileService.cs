using GloryScout.API.Services;
using GloryScout.Domain.Dtos.UserProfileDtos;


namespace GloryScout.API.Services.UserProfiles;

    public interface IUserProfileService
    {

    Task<User> GetUserByIdAsync(Guid id);
	Task<int> GetFollowersCountAsync(Guid id);
	Task<int> GetFollowingCountAsync(Guid id);
	Task FollowUserAsync(Guid followerId, Guid followeeId);
	Task UnfollowUserAsync(Guid followerId, Guid followeeId);
	Task<UserProfileDto> GetProfileasync(string id);
	Task CreatePostAsync(Guid postId, Guid userId, string description, string postUrl);
	Task DeletePostAsync(Guid postId, Guid userId);
	Task UpdateProfileAsync(Guid userId, string profileDescription, string profilePhotoUrl);
	Task<Post> GetPostByIdAsync(Guid postId);
	Task<bool> IsFollowingAsync(Guid viewerId, Guid profileId);
}

