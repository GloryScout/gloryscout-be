using FluentValidation;
using GloryScout.Domain.Dtos;
using System;
using System.Collections.Generic;


namespace GloryScout.Domain.Validators
{
	public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
	{
		private readonly List<string> _allowedUserTypes = new List<string> { "Player", "Scout" };

		public RegisterUserDtoValidator()
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

			RuleFor(x => x.ConfirmPassword)
				.NotEmpty().WithMessage("Confirm password is required.")
				.Equal(x => x.Password).WithMessage("Passwords do not match.");

			RuleFor(x => x.Nationality)
				.NotEmpty().WithMessage("Nationality is required.")
				.MaximumLength(20).WithMessage("Nationality cannot exceed 20 characters.");

			RuleFor(x => x.UserType)
				.NotEmpty().WithMessage("User type is required.")
				.Must(userType => _allowedUserTypes.Contains(userType))
				.WithMessage($"User type must be one of the following: {string.Join(", ", _allowedUserTypes)}");
		}
	}

	public class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
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

	public class VerifyUserDtoValidator : AbstractValidator<VerifyUserDto>
	{
		public VerifyUserDtoValidator()
		{
			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("Email is required.")
				.EmailAddress().WithMessage("A valid email address is required.");

			RuleFor(x => x.Code)
				.NotEmpty().WithMessage("Verification code is required.")
				.Length(6).WithMessage("Verification code must be 6 characters long.");
		}
	}
}
