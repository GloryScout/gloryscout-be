using GloryScout.API.Services.Auth;
using GloryScout.Domain.Dtos.IdentityDtos;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

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

	[HttpGet("user-info")]
	[Authorize]
	public async Task<ActionResult<object>> GetUserInfo()
	{
		var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
		if (string.IsNullOrEmpty(userId))
		{
			return Unauthorized();
		}

		var user = await _userManager.FindByIdAsync(userId);
		if (user == null)
		{
			return NotFound();
		}

		var role = User.FindFirstValue(ClaimTypes.Role);

		var userInfo = new
		{
			Id = user.Id,
			UserName = user.UserName,
			Email = user.Email,
			PhoneNumber = user.PhoneNumber,
			UserType = user.UserType,
			ProfilePhoto = user.ProfilePhoto,
			ProfileDescription = user.ProfileDescription,
			Role = role
		};

		return Ok(userInfo);
	}

	[HttpPost("change-password")]
	[Authorize]
	public async Task<IActionResult> ChangePasswordAsync(ChangePasswordDto dto)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
		if (string.IsNullOrEmpty(userId))
		{
			return Unauthorized();
		}

		// Update the DTO with the user ID from the token
		dto.Id = Guid.Parse(userId);

		var result = await _authService.ChangePasswordAsync(dto);

		if (!result.IsAuthenticated)
			return BadRequest(result.Message);

		return Ok(result);
	}

	[HttpPost("logout")]
	[Authorize]
	public async Task<IActionResult> Logout()
	{
		await _signInManager.SignOutAsync();
		return Ok("Logout success!");
	}

	/// <summary>
	/// Step 1: Receive user email and send OTP to email, store OTP in DB
	/// </summary>
	[HttpPost("send-password-reset-code")]
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
	[HttpPost("reset-password")]
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