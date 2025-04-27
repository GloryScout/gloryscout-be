using GloryScout.API.Services.Auth;
using GloryScout.Domain.Dtos.IdentityDtos;
using System.Security.Claims;

namespace GloryScout.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserProfileController : ControllerBase
{

private readonly IMapper _mapper;

public UserProfileController(IMapper mapper)
{
    _mapper = mapper;
}

//[HttpGet("{id}")]
//public async Task<IActionResult> GetUserProfile(Guid id)
//{
//    var user = await _userService.GetUserByIdAsync(id);
//    if (user == null)
//    {
//        return NotFound();
//    }
//    var profileDto = new UserProfileDto
//    {
//        Id = user.Id,
//        UserName = user.UserName,
//        ProfileDescription = user.ProfileDescription,
//        Posts = user.Posts.Select(p => p.MediaUrl).ToList(),
//        FollowersCount = await _userService.GetFollowersCountAsync(id)
//    };
//    return Ok(profileDto);
//}
}