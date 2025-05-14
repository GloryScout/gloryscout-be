using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GloryScout.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using GloryScout.API.Services.UserProfiles;
using GloryScout.Domain.Dtos.UserProfileDtos;

namespace GloryScout.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class SearchPagesController : ControllerBase
	{
		private readonly AppDbContext _context;
		private readonly IUserProfileService _userProfileService;


		public SearchPagesController(AppDbContext context , IUserProfileService userProfileService)
		{
			_context = context;
			_userProfileService = userProfileService;
		}

		// GET: api/SearchPages/players
		[HttpGet("players")]
		public async Task<ActionResult<IEnumerable<object>>> GetPlayers()
		{
			var players = await _context.Players
				.Include(p => p.User)
				.Select(p => new
				{
					id = p.Id,
					userName = p.User.UserName,
					age = p.Age,
					position = p.Position,
					dominantFoot = p.DominantFoot,
					weight = p.Weight,
					height = p.Height,
					currentTeam = p.CurrentTeam,
					userId = p.UserId,
					nationality = p.User.Nationality,
					userType = p.User.UserType,
					profilePhoto = p.User.ProfilePhoto,
					profileDescription = p.User.ProfileDescription
				})
				.ToListAsync();

			return Ok(players);
		}

		// GET: api/SearchPages/players/{id}
		[HttpGet("players/{id}")]
		public async Task<ActionResult<object>> GetPlayer(Guid id)
		{
			var player = await _context.Players
				.Include(p => p.User)
				.Where(p => p.Id == id)
				.Select(p => new
				{
					id = p.Id,
					userName = p.User.UserName,
					age = p.Age,
					position = p.Position,
					dominantFoot = p.DominantFoot,
					weight = p.Weight,
					height = p.Height,
					currentTeam = p.CurrentTeam,
					userId = p.UserId,
					nationality = p.User.Nationality,
					userType = p.User.UserType,
					profilePhoto = p.User.ProfilePhoto,
					profileDescription = p.User.ProfileDescription
				})
				.FirstOrDefaultAsync();

			if (player == null)
			{
				return NotFound();
			}

			return Ok(player);
		}

		// GET: api/SearchPages/scouts
		[HttpGet("scouts")]
		public async Task<ActionResult<IEnumerable<object>>> GetScouts()
		{
			var scouts = await _context.Scouts
				.Include(s => s.User)
				.Select(s => new
				{
					id = s.Id,
					userName = s.User.UserName,
					profileDescription = s.User.ProfileDescription,
					specialization = s.Specialization,
					experience = s.Experience,
					currentClubName = s.CurrentClubName,
					coachingSpecialty = s.CoachingSpecialty,
					userId = s.UserId,
					nationality = s.User.Nationality,
					userType = s.User.UserType,
					profilePhoto = s.User.ProfilePhoto,
				})
				.ToListAsync();

			return Ok(scouts);
		}

		// GET: api/SearchPages/scouts/{id}
		[HttpGet("scouts/{id}")]
		public async Task<ActionResult<object>> GetScout(Guid id)
		{
			var scout = await _context.Scouts
				.Include(s => s.User)
				.Where(s => s.Id == id)
				.Select(s => new
				{
					id = s.Id,
					userName = s.User.UserName,
					profileDescription = s.User.ProfileDescription,
					specialization = s.Specialization,
					experience = s.Experience,
					currentClubName = s.CurrentClubName,
					coachingSpecialty = s.CoachingSpecialty,
					userId = s.UserId,
					nationality = s.User.Nationality,
					userType = s.User.UserType,
					profilePhoto = s.User.ProfilePhoto,

				})
				.FirstOrDefaultAsync();

			if (scout == null)
			{
				return NotFound();
			}

			return Ok(scout);
		}


		[HttpGet("get-profile/{id}")]
		public async Task<IActionResult> GetUserProfileById(Guid id)
		{

			var user = await _userProfileService.GetUserByIdAsync(id);
			if (user == null)
			{
				return NotFound();
			}

			var profileDto = await _userProfileService.GetProfileasync(id.ToString());
			// determine viewer ID
			Guid? viewerId = null;
			var uid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (Guid.TryParse(uid, out var parsed)) viewerId = parsed;

			// check follow status
			var isFollowing = viewerId.HasValue ? await _userProfileService.IsFollowingAsync(viewerId.Value, id) : false;

			// wrap and return
			var response = new OtherUserProfile
			{
				Id=profileDto.Id,
				Username = profileDto.Username,
				ProfileDescription = profileDto.ProfileDescription,
				ProfilePhoto = profileDto.ProfilePhoto,
				FollowersCount = profileDto.FollowersCount,
				FollowingCount = profileDto.FollowingCount,
				Posts = profileDto.Posts,
				IsFollowing = isFollowing,
			};

			return Ok(response);
		}


		

	}
}