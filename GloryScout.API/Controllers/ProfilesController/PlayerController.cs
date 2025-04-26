using GloryScout.API.Services.PlayerServiceandCoach;
using GloryScout.Data.Repository.PlayerRepo;
using GloryScout.DTOs.Player;
using GloryScout.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GloryScout.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService;
        private readonly ILogger<PlayerController> _logger;
        private readonly IPlayerRepo _repo;

        public PlayerController(IPlayerService playerService, ILogger<PlayerController> logger, IPlayerRepo repo)
        {
            _playerService = playerService;
            _logger = logger;
            _repo = repo;
        }

        // GET: api/player/{username}
        [HttpGet("{username}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlayerProfileDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProfile(string username)
        {
            try
            {
                var result = await _playerService.GetProfile(username);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Player profile not found");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting player profile");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/player
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PlayerProfileDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateProfile([FromBody] CreatePlayerDto profileDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _playerService.CreateProfile(profileDto);
                return CreatedAtAction(nameof(GetProfile), new { username = result.Username }, result);
            }
            catch (ArgumentException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating player profile");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var players = await _repo.GetAllAsync();
            if (players.Count() == 0)
                return NotFound("No players found");
            return Ok(players);
        }
    }
}