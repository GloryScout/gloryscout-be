using GloryScout.API.Services.Auth;
using GloryScout.Domain.Dtos.IdentityDtos;
using System.Security.Claims;

namespace GloryScout.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;
    private readonly CloudinaryService _cloudinaryService;
	

	public AuthController(
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        IAuthService authService,
        IMapper mapper,
		CloudinaryService Cloudinary

		)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _authService = authService;
        _mapper = mapper;
        _cloudinaryService = Cloudinary;
    }
	[HttpPost("register-coach")]
	public async Task<IActionResult> RegisterCoachAsync([FromForm] CoachRegisterDto dto, IFormFile profilePhoto)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		var result = await _authService.RegisterCoachAsync(dto, profilePhoto);

		if (!result.IsAuthenticated)
			return BadRequest(result.Message);

		return Ok(result);
	}


	[HttpPost("register-player")]
	public async Task<IActionResult> RegisterPlayerAsync([FromForm] PlayerRegisterDto dto, IFormFile profilePhoto)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		var result = await _authService.RegisterPlayerAsync(dto, profilePhoto);

		if (!result.IsAuthenticated)
			return BadRequest(result.Message);

		return Ok(result);
	}



	[HttpPost("login")] 
    public async Task<IActionResult> GetTokenAsync([FromBody] LoginDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.LoginAsync(model);

        if (!result.IsAuthenticated)
            return BadRequest(result.Message);

        return Ok(result);
    }
    
    //[HttpGet]
    //[Authorize(Policy = "Admin")]
    //public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
    //{
    //    var modelItems = await _userManager.Users.ToListAsync();
    //    IEnumerable<UserDto> result = _mapper.Map<IEnumerable<UserDto>>(modelItems);
    //    foreach (var userDto in result.Select((value, i) => new { i, value }))
    //    {
    //       userDto.value.Role = (await _userManager.GetClaimsAsync(modelItems[userDto.i])).FirstOrDefault(c=>c.Type==ClaimTypes.Role)!.Value;
    //    }
    //    return Ok(result);
    //}

    //[HttpGet("id")]
    //public async Task<ActionResult<UserDto>> GetById(Guid id)
    //{
    //    var modelItem = await _userManager.FindByIdAsync(id.ToString());
    //    UserDto result = _mapper.Map<UserDto>(modelItem);
    //    result.Role = (await _userManager.GetClaimsAsync(modelItem!)).FirstOrDefault(c => c.Type == ClaimTypes.Role)!.Value;
    //    return Ok(result);
    //}

    //[HttpGet("own-info")]
    //[Authorize]
    //public async Task<ActionResult<UserDto>> GetOwnInfo()
    //{
    //    var currentUser = await _userManager.GetUserAsync(User);
    //    var modelItem = await _userManager.FindByIdAsync(currentUser!.Id.ToString());
    //    UserDto result = _mapper.Map<UserDto>(modelItem);
    //    result.Role = (await _userManager.GetClaimsAsync(modelItem!)).FirstOrDefault(c => c.Type == ClaimTypes.Role)!.Value;
    //    return Ok(result);
    //}

    //[HttpPut]
    //public async Task<ActionResult> UpdateAsync(Guid id, UpdateUserDto dto)
    //{
    //    if (id != dto.Id)
    //    {
    //        return BadRequest("Id not matched");
    //    }

    //    if (!ModelState.IsValid)
    //        return BadRequest(ModelState);

    //    var result = await _authService.UpdateUserAsync(dto);

    //    if (!result.IsAuthenticated)
    //        return BadRequest(result.Message);

    //    return Ok(result);
    //}

    //[HttpPost("change-password")]
    //public async Task<IActionResult> ChangePasswordAsync(ChangePasswordDto dto)
    //{
    //    if (!ModelState.IsValid)
    //        return BadRequest(ModelState);

    //    var result = await _authService.ChangePasswordAsync(dto);

    //    if (!result.IsAuthenticated)
    //        return BadRequest(result.Message);

    //    return Ok(result);
    //}
    
    //[HttpDelete]
    //[Authorize(Policy = "Admin")]
    //public async Task<IActionResult> DeleteUser(Guid userId)
    //{
    //    var user = await _userManager.FindByIdAsync(userId.ToString());

    //    if (user == null)
    //        return NotFound();

    //    var result = await _userManager.DeleteAsync(user);

    //    if (!result.Succeeded)
    //        return BadRequest("Not Deleted");

    //    return Ok();
    //}

    //[HttpPost]
    //[Route("logout")]
    //[Authorize]
    //public async Task<IActionResult> Logout()
    //{
    //    await _signInManager.SignOutAsync();
    //    return Ok("Logout success!");
    //}

    


    
	/// <summary>
	/// Step 1: Receive user email and send OTP to email, store OTP in DB
	/// </summary>
	[HttpPost("Send-Password-ResetCode")]
	public async Task<IActionResult> SendPasswordResetCode([FromBody] SendResetCodeDto dto)
	{
		if (string.IsNullOrEmpty(dto.Email))
			return BadRequest("Email should not be null or empty");

		await _authService.SendPasswordResetCodeAsync(dto.Email);
		return Ok("OTP sent successfully");
	}

	

	/// <summary>
	/// Step 2: Reset password and verify the OTP
	/// </summary>
	[HttpPost("Reset-Password")]
	public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
	{
		if (string.IsNullOrEmpty(dto.Email)
			|| string.IsNullOrEmpty(dto.OTP)
			|| string.IsNullOrEmpty(dto.NewPassword))
			return BadRequest("Email, OTP code, and new password are required");

		var result = await _authService.ResetPasswordAsync(dto.Email, dto.OTP, dto.NewPassword);
		if (!result.Succeeded)
			return BadRequest("Password reset failed");

		return Ok("Password reset successful");
	}
}