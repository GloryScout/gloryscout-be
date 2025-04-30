using AutoMapper;
using GloryScout.Data.Models.Followers;
using GloryScout.Domain.Dtos.UserProfileDtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GloryScout.API.Services.UserProfiles
{
    public class UserProfileService : IUserProfileService
    {
		private readonly AppDbContext _context;
		private readonly IMapper _mapper;

		public UserProfileService(AppDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task FollowUserAsync(Guid followerId, Guid followeeId)
		{
			// Check if the follow relationship already exists
			if (await _context.UserFollowings.AnyAsync(uf => uf.FollowerId == followerId && uf.FolloweeId == followeeId))
			{
				throw new InvalidOperationException("You are already following this user.");
			}

			// Create a new follow relationship
			var follow = new UserFollowings
			{
				FollowerId = followerId,
				FolloweeId = followeeId,
				FollowedAt = DateTime.Now
			};

			_context.UserFollowings.Add(follow);
			await _context.SaveChangesAsync();
		}

		public async Task UnfollowUserAsync(Guid followerId, Guid followeeId)
		{
			var follow = await _context.UserFollowings
				.FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FolloweeId == followeeId);
			if (follow == null)
				return;

			_context.UserFollowings.Remove(follow);
			await _context.SaveChangesAsync();
		}

		public async Task<int> GetFollowersCountAsync(Guid id)
		{
			// Check if the user exists
			var userExists = await _context.Users.AnyAsync(u => u.Id == id);
			if (!userExists)
			{
				throw new InvalidOperationException("User not found.");
			}

			// Count followers
			var count = await _context.UserFollowings
				.CountAsync(uf => uf.FolloweeId == id);

			return count; // Returns 0 if no followers
		}
		

		public async Task<User> GetUserByIdAsync(Guid id)
		{
			return await _context.Users
				.AsNoTracking()
				.FirstOrDefaultAsync(u => u.Id == id);
		}

		public async Task<UserProfileDto> GetProfileasync(string id)
		{
			if (!Guid.TryParse(id, out var userId))
				throw new ArgumentException("Invalid user id format.", nameof(id));

			var user = await _context.Users
				.AsNoTracking()
				.Include(u => u.Posts)
				.FirstOrDefaultAsync(u => u.Id == userId);

			if (user == null)
				return null;
			UserProfileDto profile ;
			try {
				var Profile = new UserProfileDto
				{
					Id = user.Id,
					Username = user.UserName,
					ProfileDescription = user.ProfileDescription,
					Posts = user.Posts.Select(p => new PostDto
					{
						Id = p.Id,
						Description=p.Description,
						PosrUrl = p.PosrUrl
					}).ToList(),
					FollowersCount = await GetFollowersCountAsync(user.Id)

				};
				profile = _mapper.Map<UserProfileDto>(Profile);
			}
			catch (Exception ex)
			{
				throw new Exception("An error occurred while retrieving the user profile.", ex);
			}
			return profile;
		}

		public async Task CreatePostAsync(Guid postId, Guid userId, string description, string postUrl)
		{
			var post = new Post
			{
				Id = postId,
				Description = description,
				CreatedAt = DateTime.Now,
				UserId = userId,
				PosrUrl = postUrl
			};
			_context.Posts.Add(post);
			await _context.SaveChangesAsync();
		}

		public async Task DeletePostAsync(Guid postId, Guid userId)
		{
			var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
			if (post == null)
			{
				throw new InvalidOperationException("Post not found.");
			}
			if (post.UserId != userId)
			{
				throw new UnauthorizedAccessException("You are not authorized to delete this post.");
			}
			_context.Posts.Remove(post);
			await _context.SaveChangesAsync();
		}

		public async Task<Post> GetPostByIdAsync(Guid postId)
		{
			return await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
		}

		public async Task UpdateProfileAsync(Guid userId, string profileDescription, string profilePhotoUrl)
		{
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
			if (user == null)
			{
				throw new InvalidOperationException("User not found.");
			}
			if (!string.IsNullOrEmpty(profileDescription))
			{
				user.ProfileDescription = profileDescription;
			}
			if (!string.IsNullOrEmpty(profilePhotoUrl))
			{
				user.ProfilePhoto = profilePhotoUrl;
			}
			await _context.SaveChangesAsync();
		}
	}
}
