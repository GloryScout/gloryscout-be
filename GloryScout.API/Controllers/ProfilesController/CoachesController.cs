// ملف API/Controllers/CoachesController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GloryScout.Data;
using GloryScout.Data.Models.Entities;
using GloryScout.Data.Repository.PlayerRepo;
using GloryScout.Data.Repository.ScoutRepo;

[ApiController]
[Route("api/[controller]")]
public class CoachesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IScoutRepo _repo;
    public CoachesController(AppDbContext context , IScoutRepo Repo)
    {
        _context = context;
        this._repo = Repo;
    }

    // GET: api/coaches/{username}
    [HttpGet("{username}")]
    public async Task<IActionResult> GetCoachProfile(string username)
    {
        var profile = await _context.CoachProfiles
            .Include(c => c.MediaItems)
            .FirstOrDefaultAsync(c => c.Username == username);

        if (profile == null)
        {
            return NotFound();
        }

        return Ok(profile);
    }

    // POST: api/coaches
    [HttpPost]
    public async Task<IActionResult> CreateCoachProfile([FromBody] CoachProfile profile)
    {
        _context.CoachProfiles.Add(profile);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCoachProfile), new { username = profile.Username }, profile);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var scouts = await _repo.GetAllAsync();
        if(scouts.Count() == 0)
            return NotFound("No Scouts found");
        return Ok(scouts);
    }
}