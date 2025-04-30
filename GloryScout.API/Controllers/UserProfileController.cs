using GloryScout.API.Services.Auth;
using GloryScout.Domain.Dtos.IdentityDtos;
using System.Security.Claims;
using GloryScout.API.Services.UserProfiles;
using GloryScout.API.Services;
using GloryScout.Domain.Dtos.UserProfileDtos;
using System.Runtime.InteropServices;

namespace GloryScout.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserProfileController : ControllerBase
{

private readonly IMapper _mapper;
	private readonly IUserProfileService _userProfileService;
	private readonly CloudinaryService _cloudinaryService;

	public UserProfileController(IMapper mapper, IUserProfileService userProfileService, CloudinaryService cloudinaryService)
	{
		_mapper = mapper;
		_userProfileService = userProfileService;
		_cloudinaryService = cloudinaryService;
	}

	[HttpGet("get-profile{id}")]
    public async Task<IActionResult> GetUserProfile(Guid id)
    {
        var user = await _userProfileService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        var profileDto = await _userProfileService.GetProfileasync(id.ToString());
		return Ok(profileDto);
    }

	[HttpPost("{followerId}/follows/{followeeId}")]
	public async Task<IActionResult> FollowUser(Guid followerId,Guid followeeId)
	{
		try
		{
			await _userProfileService.FollowUserAsync(followerId, followeeId);
			return Ok(new { Message = "Successfully followed user." });
		}
		catch (InvalidOperationException ex)
		{
			return BadRequest(new { Error = ex.Message });
		}
		catch (Exception)
		{
			return StatusCode(500, new { Error = "An error occurred while processing your request." });
		}
	}

	[HttpPost("{followerId}/Unfollows/{followeeId}")]
	public async Task<IActionResult> UnfollowUser(Guid followerId, Guid followeeId)
	{
		try
		{
			await _userProfileService.UnfollowUserAsync(followerId, followeeId);
			return Ok(new { Message = "Successfully unfollowed user." });
		}
		catch (InvalidOperationException ex)
		{
			return BadRequest(new { Error = ex.Message });
		}
		catch (Exception)
		{
			return StatusCode(500, new { Error = "An error occurred while processing your request." });
		}
	}


	[HttpPost("create-post")]
	public async Task<IActionResult> CreatePost([FromForm] CreatePostDto dto , IFormFile file)
	{
		Console.WriteLine($"UserId: {dto.UserId}, Description: {dto.Description}, File: {file?.FileName}");
		if (!ModelState.IsValid)
		{
			foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
			{
				Console.WriteLine($"ModelState Error: {error.ErrorMessage}");
			}
			return BadRequest(ModelState);
		}
		if (string.IsNullOrEmpty(dto.Description))
		{
			return BadRequest(new { Error = "Description is required." });
		}
		try
		{
			var userId = dto.UserId;
			string description = dto.Description;
			if (file == null || file.Length == 0)
			{
				return BadRequest(new { Error = "File is required." });
			}

			var postId = Guid.NewGuid();
			var postUrl = await _cloudinaryService.UploadPostAsync(file, userId.ToString(), postId.ToString());

			if (postUrl == null)
			{
				return BadRequest(new { Error = "Failed to upload post to Cloudinary." });
			}

			var postDto = new PostDto
			{
				Id = postId,
				Description = description,
				PosrUrl = postUrl
			};

			var post = _mapper.Map<Post>(postDto);
			await _userProfileService.CreatePostAsync(post.Id, userId, post.Description, post.PosrUrl);

			return Ok(new { Message = "Post created successfully.", PostId = postId });
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { Error = "An error occurred while creating the post: " + ex.Message });
		}
	}

	[HttpDelete("delete-post/{postId}/{userId}")]
	public async Task<IActionResult> DeletePost(Guid postId,Guid userId)
	{
		try
		{
			var user = await _userProfileService.GetUserByIdAsync(userId);
			var post = await _userProfileService.GetPostByIdAsync(postId);

			if (post == null)
			{
				return NotFound(new { Error = "Post not found." });
			}

			if (post.UserId != user.Id)
			{
				return Unauthorized(new { Error = "You are not authorized to delete this post." });
			}

			var deleteResult = await _cloudinaryService.DeletePhotoAsync(post.PosrUrl);
			

			await _userProfileService.DeletePostAsync(postId, userId);
			return Ok(new { Message = "Post deleted successfully." });
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { Error = "An error occurred while deleting the post: " + ex.Message });
		}
	}


	[HttpPut("edit")]
	public async Task<IActionResult> EditProfile([FromForm] EditProfileDto dto, IFormFile newProfilePic)
	{
		try
		{
			// Await the task to get the actual User object
			var user = await _userProfileService.GetUserByIdAsync(dto.UserId);

			if (user == null)
			{
				return NotFound(new { Error = "User not found." });
			}

			// Upload the new profile picture first
			var newProfilePhotoUrl = await _cloudinaryService.UploadProfilePhotoAsync(newProfilePic, dto.UserId.ToString());
			if (newProfilePhotoUrl == null)
			{
				return BadRequest(new { Error = "Failed to upload profile picture." });
			}

			// If there’s an existing profile photo, delete it after successful upload
			if (!string.IsNullOrEmpty(user.ProfilePhoto))
			{
				await _cloudinaryService.DeletePhotoAsync(user.ProfilePhoto);
			}

			// Update the user profile in the database with the new description and photo URL (if updated)
			await _userProfileService.UpdateProfileAsync(dto.UserId, dto.ProfileDescription, newProfilePhotoUrl);
			return Ok(new { Message = "Profile updated successfully." });
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { Error = "An error occurred while updating the profile: " + ex.Message });
		}
	}
}