using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GloryScout.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GloryScout.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SearchPagesController : ControllerBase
	{
		private readonly AppDbContext _context;

		public SearchPagesController(AppDbContext context)
		{
			_context = context;
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
					clubName = s.ClubName,
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
					clubName = s.ClubName,
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
	}
}