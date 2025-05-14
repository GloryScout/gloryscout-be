using Azure.Messaging;
using GloryScout.API.Services.Posts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GloryScout.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class PostController : ControllerBase
	{
		private readonly IPostServices _postServices;
		public PostController(IPostServices postServices)
		{
			_postServices = postServices;
		}
		

		

		// GET: api/posts/{postId}
		[HttpGet("{postId:guid}")]
		public async Task<IActionResult> GetPostById([FromRoute] Guid postId)
		{
			try
			{
				var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
				var post = await _postServices.GetPostByIdWithDetailsAsync(postId, userId);
				return Ok(post);
			}
			catch (InvalidOperationException ex)
			{
				return NotFound(new { error = ex.Message });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { error = "An error occurred while retrieving the post: " + ex.Message });
			}
		}

		// POST: api/posts/{postId}/like
		[HttpPost("{postId:guid}/like")]
		public async Task<IActionResult> LikePost([FromRoute] Guid postId)
		{
			var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			try
			{
				var like = await _postServices.LikePostAsync(postId, userId);
				return CreatedAtAction(null, new { postId = like.PostId, userId = like.UserId });
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { error = ex.Message });
			}
		}

		// DELETE: api/posts/{postId}/like
		[HttpDelete("{postId:guid}/like")]
		public async Task<IActionResult> UnlikePost([FromRoute] Guid postId)
		{
			var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			try
			{
				await _postServices.UnlikePostAsync(postId, userId);
				return Ok("like removed from post");
			}
			catch (InvalidOperationException ex)
			{
				return NotFound(new { error = ex.Message });
			}
		}

		// GET: api/posts/{postId}/comments?page={page}&pageSize={pageSize}
		[HttpGet("{postId:guid}/comments")]
		public async Task<IActionResult> GetComments([FromRoute] Guid postId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
		{
			try
			{
				var comments = await _postServices.GetCommentsAsync(postId, page, pageSize);
				return Ok(comments);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(new { error = ex.Message });
			}
		}

		// POST: api/posts/{postId}/comments
		[HttpPost("{postId:guid}/comment")]
		public async Task<IActionResult> AddComment([FromRoute] Guid postId, [FromBody] CommentRequest request)
		{
			var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			try
			{
				var comment = await _postServices.AddCommentAsync(postId, userId, request.CommentText);
				return CreatedAtAction(nameof(GetComments), new { postId = postId }, comment);
			}
			catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
			{
				return BadRequest(new { error = ex.Message });
			}
		}

		// DELETE: api/comments/{commentId}
		[HttpDelete("/api/comments/{commentId:guid}")]
		public async Task<IActionResult> DeleteComment([FromRoute] Guid commentId)
		{
			var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			try
			{
				await _postServices.DeleteCommentAsync(commentId, userId);
				return Ok("comment deleted successfully");

			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { error = ex.Message });
			}
		}

		// PUT: api/posts/{postId}
		[HttpPut("Upate-post/{postId:guid}")]
		public async Task<IActionResult> UpdatePost([FromRoute] Guid postId, [FromBody] UpdatePostRequest request)
		{
			var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			try
			{
				var updatedPost = await _postServices.UpdatePostAsync(postId, userId, request.Description);
				return Ok(updatedPost);
			}
			catch (InvalidOperationException ex)
			{
				// Return 403 Forbidden if user is not authorized
				if (ex.Message.Contains("not authorized"))
					return StatusCode(403, new { error = ex.Message });

				// Return 404 Not Found if post doesn't exist
				return NotFound(new { error = ex.Message });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { error = "An error occurred while updating the post: " + ex.Message });
			}
		}


		public class UpdatePostRequest
		{
			public string Description { get; set; }
		}

	}
}