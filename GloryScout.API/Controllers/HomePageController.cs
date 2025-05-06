using GloryScout.API.Services.Posts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static GloryScout.API.Services.Posts.PostServices;

namespace GloryScout.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HomePageController : ControllerBase
    {
		private readonly IPostServices _postServices;

		public HomePageController(IPostServices postServices)
		{
			_postServices = postServices;
		}



		/// <summary>
		/// GET: api/HomePage/feed?
		///     lastLikesCount={int}&
		///     lastCreatedAt={datetime}&
		///     limit={int}
		///
		/// If you omit both cursor values on first call, you get the top page.
		/// Subsequent calls must pass BOTH lastLikesCount & lastCreatedAt.
		/// </summary>
		[HttpGet("feed")]
		public async Task<IActionResult> GetFeed(
			[FromQuery] int? lastLikesCount,
			[FromQuery] DateTime? lastCreatedAt,
			[FromQuery] int limit = 20)
		{
			if (limit < 1)
				return BadRequest("Limit must be at least 1.");

			// 1) Extract current user ID
			var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (!Guid.TryParse(userIdClaim, out var userId))
				return Unauthorized();

			// 2) Build cursor if both parts are provided
			FeedCursor? cursor = null;
			if (lastLikesCount.HasValue && lastCreatedAt.HasValue)
			{
				cursor = new FeedCursor
				{
					LastLikesCount = lastLikesCount.Value,
					LastCreatedAt = lastCreatedAt.Value
				};
			}
			else if (lastLikesCount.HasValue || lastCreatedAt.HasValue)
			{
				// If only one part is present, that’s invalid
				return BadRequest("You must supply both lastLikesCount and lastCreatedAt, or neither.");
			}

			// 3) Fetch the page
			var result = await _postServices.GetFeedAsync(userId, cursor, limit);

			return Ok(result);
		}
	}

	public class CommentRequest
	{
		public string CommentText { get; set; }
	}
	

}

