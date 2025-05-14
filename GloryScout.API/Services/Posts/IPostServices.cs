using static GloryScout.API.Services.Posts.PostServices;

namespace GloryScout.API.Services.Posts
{
	public interface IPostServices
	{

		Task<FeedResult> GetFeedAsync(Guid currentUserId, FeedCursor lastCursor, int limit);
		Task<Like> LikePostAsync(Guid postId, Guid userId);

		Task UnlikePostAsync(Guid postId, Guid userId);

		// Returns full comment DTOs including author info
		Task<List<CommentDto>> GetCommentsAsync(Guid postId, int page, int pageSize);

		Task<CommentDto> AddCommentAsync(Guid postId, Guid userId, string commentText);

		Task DeleteCommentAsync(Guid commentId, Guid userId);

		Task<PostDetailDto> GetPostByIdWithDetailsAsync(Guid postId, Guid currentUserId);
		Task<PostDetailDto> UpdatePostAsync(Guid postId, Guid userId, string description);

	}

	
}
