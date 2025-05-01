using GloryScout.API.Services.Posts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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



		// GET: api/posts/feed?lastCursor={timestamp}&limit={limit}
		[HttpGet("feed")]
		public async Task<IActionResult> GetFeed([FromQuery] DateTime? lastCursor, [FromQuery] int limit = 20)
		{
			var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			var result = await _postServices.GetFeedAsync(userId, lastCursor, limit);
			return Ok(result);
		}

	
	}

	public class CommentRequest
	{
		public string CommentText { get; set; }
	}
}

