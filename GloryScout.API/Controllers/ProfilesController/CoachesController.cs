// ملف API/Controllers/CoachesController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GloryScout.Data;
using GloryScout.Data.Models.Entities;

[ApiController]
[Route("api/[controller]")]
public class CoachesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CoachesController(AppDbContext context)
    {
        _context = context;
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
}