using System.Collections.Generic;
using GloryScout.Domain.Dtos.HomePageDtos;

namespace GloryScout.API.Services.Posts
{
	public class PostServices : IPostServices
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly CloudinaryService _cloudinaryService;
		private readonly AppDbContext _context;
		private const int GlobalSlotCount = 7;
		public PostServices(IUnitOfWork unitOfWork, IMapper mapper, CloudinaryService cloudinaryService, AppDbContext context)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_cloudinaryService = cloudinaryService;
			_context = context;
		}
		public class FeedResult
		{
			public List<FeedPostsDto> Posts { get; set; } = new List<FeedPostsDto>();
			public DateTime? NextCursor { get; set; }
			public bool HasMore { get; set; }
		}


		#region necessary Dtos
		public class PostDetailDto
		{
			public Guid Id { get; set; }
			public string Description { get; set; }
			public string PostUrl { get; set; }
			public DateTime CreatedAt { get; set; }
			public UserBasicInfoDto User { get; set; }
			public int LikesCount { get; set; }
			public bool IsLikedByCurrentUser { get; set; }
			public List<CommentDto> Comments { get; set; }
		}
		public class CommentDto
		{
			public Guid Id { get; set; }
			public string CommentedText { get; set; }
			public DateTime CreatedAt { get; set; }
			public UserBasicInfoDto User { get; set; }
		}

		public class UserBasicInfoDto
		{
			public Guid Id { get; set; }
			public string Username { get; set; }
			public string ProfilePhoto { get; set; }
		}
#endregion


		/// <summary>
		/// Retrieves the most recent posts across the whole system,
		/// sorted by CreatedAt descending.
		/// </summary>
		public async Task<List<Post>> GetLatestFeedAsync(int count)
		{
			return await _context.Posts
				.Include(p => p.User)
				.OrderByDescending(p => p.CreatedAt)
				.Take(count)
				.ToListAsync();
		}

		/// <summary>
		/// Retrieves the next chunk of the user's feed using cursor-based pagination.
		/// On the first load (no cursor), it returns the top global posts (up to GlobalSlotCount)
		/// plus followee posts to fill the remaining slots. On subsequent loads (cursor provided),
		/// it returns followee posts older than the cursor timestamp, excluding any already sent.
		/// </summary>
		/// <param name="userId">Authenticated user's ID.</param>
		/// <param name="lastCursor">Timestamp of the last-seen post (null for first load).</param>
		/// <param name="limit">Maximum number of posts to return.</param>
		public async Task<FeedResult> GetFeedAsync(Guid userId, DateTime? lastCursor, int limit)
		{
			if (limit < 1)
				throw new ArgumentException("Limit must be >= 1.");

			var result = new FeedResult();
			int remaining = limit;
			var sentGlobalIds = new List<Guid>();

			// First load: include global posts
			if (!lastCursor.HasValue)
			{
				var globalPosts = await GetLatestFeedAsync(GlobalSlotCount);
				var globalPostDtos = await _context.Posts
					.Include(p => p.User)
					.Include(p => p.Likes)
					.Include(p => p.Comments)
					.OrderByDescending(p => p.CreatedAt)
					.Take(GlobalSlotCount)
					.Select(p => new FeedPostsDto
					{
						Id = p.Id,
						Description = p.Description,
						PosrUrl = p.PosrUrl,
						CreatedAt = p.CreatedAt,
						Username = p.User.UserName,
						UserProfilePicture = p.User.ProfilePhoto,
						LikesCount = p.Likes.Count,
						CommentsCount = p.Comments.Count,
						IsLikedByCurrentUser = p.Likes.Any(l => l.UserId == userId)
					})
					.ToListAsync();
				result.Posts.AddRange(globalPostDtos);
				sentGlobalIds = globalPosts.Select(p => p.Id).ToList();
				remaining -= globalPosts.Count;
			}

			// Build followee query, excluding already sent global posts
			IQueryable<Post> followeeQuery = _context.Posts
				.Include(p => p.User)
				.Where(p =>
					_context.UserFollowings
						.Any(uf => uf.FollowerId == userId && uf.FolloweeId == p.UserId)
					&& !sentGlobalIds.Contains(p.Id)
				)
				.OrderByDescending(p => p.CreatedAt);

			if (lastCursor.HasValue)
			{
				followeeQuery = followeeQuery
					.Where(p => p.CreatedAt < lastCursor.Value);
			}
			
			List<FeedPostsDto> followeePostDtos = null;

			if (remaining > 0)
			{
				var followeePosts = await followeeQuery
					.Take(remaining)
					.ToListAsync();
				followeePostDtos = await followeeQuery
					.Include(p => p.Likes)
					.Include(p => p.Comments)
					.Take(remaining)
					.Select(p => new FeedPostsDto
					{
						Id = p.Id,
						Description = p.Description,
						PosrUrl = p.PosrUrl,
						CreatedAt = p.CreatedAt,
						Username = p.User.UserName,
						UserProfilePicture = p.User.ProfilePhoto,
						LikesCount = p.Likes.Count,
						CommentsCount = p.Comments.Count,
						IsLikedByCurrentUser = p.Likes.Any(l => l.UserId == userId)
					})
					.ToListAsync();
				result.Posts.AddRange(followeePostDtos);
			}

			if (result.Posts.Any())
			{
				result.NextCursor = result.Posts.Last().CreatedAt;
				result.HasMore = result.Posts.Count >= limit;
			}
			else
			{
				result.HasMore = false;
			}

			return result;
		}



		/// <summary>
		/// Adds a like to the specified post by the authenticated user if not already liked.
		/// </summary>
		/// <param name="postId">The ID of the post to like.</param>
		/// <param name="userId">The ID of the authenticated user.</param>
		/// <returns>The created Like object.</returns>
		public async Task<Like> LikePostAsync(Guid postId, Guid userId)
		{
			var post = await _context.Posts.FindAsync(postId);
			if (post == null)
			{
				throw new InvalidOperationException("Post not found.");
			}

			var existingLike = await _context.Likes
				.FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);
			if (existingLike != null)
			{
				throw new InvalidOperationException("You have already liked this post.");
			}

			var newLike = new Like
			{
				PostId = postId,
				UserId = userId,
				LikedAt = DateTime.UtcNow
			};

			_context.Likes.Add(newLike);
			await _context.SaveChangesAsync();
			return newLike;
		}

		/// <summary>
		/// Removes the authenticated user's like from the specified post.
		/// </summary>
		/// <param name="postId">The ID of the post to unlike.</param>
		/// <param name="userId">The ID of the authenticated user.</param>
		public async Task UnlikePostAsync(Guid postId, Guid userId)
		{
			var like = await _context.Likes
				.FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);
			if (like == null)
			{
				throw new InvalidOperationException("Like not found.");
			}

			_context.Likes.Remove(like);
			await _context.SaveChangesAsync();
		}

		/// <summary>
		/// Retrieves a paginated list of comments for the specified post, sorted by creation time.
		/// </summary>
		/// <param name="postId">The ID of the post.</param>
		/// <param name="page">The page number (1-based).</param>
		/// <param name="pageSize">The number of comments per page.</param>
		/// <returns>A list of comments with author information.</returns>
		public async Task<List<CommentDto>> GetCommentsAsync(Guid postId, int page, int pageSize)
		{
			if (page < 1 || pageSize < 1)
				throw new ArgumentException("Page and pageSize must be positive integers.");

			return await _context.Comments
				.Include(c => c.User)
				.Where(c => c.PostId == postId)
				.OrderBy(c => c.CreatedAt)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.Select(c => new CommentDto
				{
					Id = c.Id,
					CommentedText = c.CommentedText,
					CreatedAt = c.CreatedAt,
					User = new UserBasicInfoDto
					{
						Id = c.User.Id,
						Username = c.User.UserName,
						ProfilePhoto = c.User.ProfilePhoto
					}
				})
				.ToListAsync();
		}

		/// <summary>
		/// Adds a new comment to the specified post by the authenticated user.
		/// </summary>
		/// <param name="postId">The ID of the post to comment on.</param>
		/// <param name="userId">The ID of the authenticated user.</param>
		/// <param name="commentText">The text of the comment.</param>
		/// <returns>The created Comment object.</returns>
		public async Task<CommentDto> AddCommentAsync(Guid postId, Guid userId, string commentText)
		{
			if (string.IsNullOrWhiteSpace(commentText))
				throw new ArgumentException("Comment text cannot be empty.");

			var post = await _context.Posts.FindAsync(postId);
			if (post == null)
				throw new InvalidOperationException("Post not found.");

			var comment = new Comment
			{
				PostId = postId,
				UserId = userId,
				CommentedText = commentText,
				CreatedAt = DateTime.UtcNow
			};

			_context.Comments.Add(comment);
			await _context.SaveChangesAsync();

			// re-load User for profile info
			await _context.Entry(comment).Reference(c => c.User).LoadAsync();

			return new CommentDto
			{
				Id = comment.Id,
				CommentedText = comment.CommentedText,
				CreatedAt = comment.CreatedAt,
				User = new UserBasicInfoDto
				{
					Id = comment.User.Id,
					Username = comment.User.UserName,
					ProfilePhoto = comment.User.ProfilePhoto
				}
			};
		}

		/// <summary>
		/// Deletes the specified comment if the authenticated user is the author.
		/// </summary>
		/// <param name="commentId">The ID of the comment to delete.</param>
		/// <param name="userId">The ID of the authenticated user.</param>
		public async Task DeleteCommentAsync(Guid commentId, Guid userId)
		{
			var comment = await _context.Comments
				.FirstOrDefaultAsync(c => c.Id == commentId);
			if (comment == null)
			{
				throw new InvalidOperationException("Comment not found.");
			}

			if (comment.UserId != userId)
			{
				throw new InvalidOperationException("You are not authorized to delete this comment.");
			}

			_context.Comments.Remove(comment);
			await _context.SaveChangesAsync();
		}






		/// <summary>
		/// Retrieves a specific post with detailed information including all comments and like count
		/// </summary>
		public async Task<PostDetailDto> GetPostByIdWithDetailsAsync(Guid postId, Guid currentUserId)
		{
			var post = await _context.Posts
				.Include(p => p.User)
				.Include(p => p.Likes)
				.Include(p => p.Comments)
				.ThenInclude(c => c.User)
				.FirstOrDefaultAsync(p => p.Id == postId);

			if (post == null)
				throw new InvalidOperationException("Post not found.");

			return new PostDetailDto
			{
				Id = post.Id,
				Description = post.Description,
				PostUrl = post.PosrUrl,
				CreatedAt = post.CreatedAt,
				User = new UserBasicInfoDto
				{
					Id = post.User.Id,
					Username = post.User.UserName,
					ProfilePhoto = post.User.ProfilePhoto
				},
				LikesCount = post.Likes.Count,
				IsLikedByCurrentUser = post.Likes.Any(l => l.UserId == currentUserId),
				Comments = post.Comments
					.OrderBy(c => c.CreatedAt)
					.Select(c => new CommentDto
					{
						Id = c.Id,
						CommentedText = c.CommentedText,
						CreatedAt = c.CreatedAt,
						User = new UserBasicInfoDto
						{
							Id = c.User.Id,
							Username = c.User.UserName,
							ProfilePhoto = c.User.ProfilePhoto
						}
					})
					.ToList()
			};	
		}
	}
}
