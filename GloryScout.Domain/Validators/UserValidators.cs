using FluentValidation;
using GloryScout.Domain.Dtos;
using GloryScout.Domain.Dtos.IdentityDtos;
using System;
using System.Collections.Generic;


namespace GloryScout.Domain.Validators
{
	public class RegisterPlayerDtoValidator : AbstractValidator<PlayerRegisterDto>
	{
		private readonly List<string> _allowedUserTypes = new List<string> { "Player", "Scout" };

		public RegisterPlayerDtoValidator()
		{
			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("Email is required.")
				.EmailAddress().WithMessage("A valid email address is required.");

			RuleFor(x => x.Password)
				.NotEmpty().WithMessage("Password is required.")
				.MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
				.Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
				.Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
				.Matches("[0-9]").WithMessage("Password must contain at least one number.")
				.Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

		}
	}

	public class RegisterScoutDtoValidator : AbstractValidator<CoachRegisterDto>
	{
		private readonly List<string> _allowedUserTypes = new List<string> { "Player", "Scout" };

		public RegisterScoutDtoValidator()
		{
			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("Email is required.")
				.EmailAddress().WithMessage("A valid email address is required.");

			RuleFor(x => x.Password)
				.NotEmpty().WithMessage("Password is required.")
				.MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
				.Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
				.Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
				.Matches("[0-9]").WithMessage("Password must contain at least one number.")
				.Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

		}
	}
	public class LoginUserDtoValidator : AbstractValidator<LoginDto>
	{
		public LoginUserDtoValidator()
		{
			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("Email is required.")
				.EmailAddress().WithMessage("A valid email address is required.");

			RuleFor(x => x.Password)
				.NotEmpty().WithMessage("Password is required.");
		}
	}

	//public class VerifyUserDtoValidator : AbstractValidator<VerifyUserDto>
	//{
	//	public VerifyUserDtoValidator()
	//	{
	//		RuleFor(x => x.Email)
	//			.NotEmpty().WithMessage("Email is required.")
	//			.EmailAddress().WithMessage("A valid email address is required.");

	//		RuleFor(x => x.Code)
	//			.NotEmpty().WithMessage("Verification code is required.")
	//			.Length(6).WithMessage("Verification code must be 6 characters long.");
	//	}
	//}
}
