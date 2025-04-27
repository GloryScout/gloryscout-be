using GloryScout.API.Services;
//using GloryScout.Domain.Dtos.UserProfileDtos;


namespace GloryScout.API.Services.UserProfiles;

    public interface IUserProfileService
    {

        Task<User> GetUserByIdAsync(Guid id);
        Task<int> GetFollowersCountAsync(Guid id);
        Task FollowUserAsync(Guid followerId, Guid followeeId);
        Task UnfollowUserAsync(Guid followerId, Guid followeeId);
        //Task<PlayerProfileDto> GetProfile(string id);
        //Task<PlayerProfileDto> CreateProfile(user UserProfileDto);
    }

