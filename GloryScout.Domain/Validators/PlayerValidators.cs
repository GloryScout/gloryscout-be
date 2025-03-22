using FluentValidation;
using GloryScout.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GloryScout.Domain.Validators
{
	public class CreatePlayerProfileDtoValidator : AbstractValidator<CreatePlayerProfileDto>
	{
		private readonly string[] _validPositions = { "Goalkeeper", "Defender", "Midfielder", "Forward" };
		private readonly string[] _validFeet = { "Left", "Right", "Both" };

		public CreatePlayerProfileDtoValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Name is required.")
				.MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

			RuleFor(x => x.Position)
				.NotEmpty().WithMessage("Position is required.")
				.Must(position => _validPositions.Contains(position))
				.WithMessage($"Position must be one of the following: {string.Join(", ", _validPositions)}");

			RuleFor(x => x.DominantFoot)
				.Must(foot => string.IsNullOrEmpty(foot) || _validFeet.Contains(foot))
				.WithMessage($"Dominant foot must be one of the following: {string.Join(", ", _validFeet)}");

			RuleFor(x => x.PhoneNumber)
				.Must(BeAValidPhoneNumber)
				.When(x => !string.IsNullOrEmpty(x.PhoneNumber))
				.WithMessage("Phone number must be in a valid format.");

			RuleFor(x => x.ProfileDescription)
				.MaximumLength(1500).WithMessage("Bio cannot exceed 1500 characters.")
				.Must(NotContainEmailOrPhone)
				.WithMessage("Bio cannot contain email addresses or phone numbers.!!"); ;

			RuleFor(x => x.Weight)
				.GreaterThan(0).WithMessage("Weight must be greater than 0.")
				.LessThan(200).WithMessage("Weight must be less than 200 kg.");

			RuleFor(x => x.Height)
				.GreaterThan(0).WithMessage("Height must be greater than 0.")
				.LessThan(300).WithMessage("Height must be less than 300 cm.");

			RuleFor(x => x.CurrentTeam)
				.MaximumLength(110).WithMessage("Current team cannot exceed 110 characters.");
		}

		private bool NotContainEmailOrPhone(string description)
		{
			//in case of an empty bio return true
			if (string.IsNullOrEmpty(description))
				return true;

			// Regular expression for detecting email addresses
			var emailRegex = new Regex(@"[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+");

			// Regex for detecting Egyptian phone numbers (e.g., +201015374542 or 01015374542)
			var phoneRegex = new Regex(@"(?:(?:\+20)|0)1[0125]\d{8}");

			return !emailRegex.IsMatch(description) && !phoneRegex.IsMatch(description);
		}

		private bool BeAValidPhoneNumber(string phoneNumber)
		{
			if (string.IsNullOrEmpty(phoneNumber))
				return true;

			// Simple regex for international phone number format
			return Regex.IsMatch(phoneNumber, @"^\+?[0-9\s\-\(\)]{10,20}$");
		}
	}

	public class UpdatePlayerProfileDtoValidator : AbstractValidator<UpdatePlayerProfileDto>
	{
		private readonly string[] _validPositions = { "Goalkeeper", "Defender", "Midfielder", "Forward" };
		private readonly string[] _validFeet = { "Left", "Right", "Both" };

		public UpdatePlayerProfileDtoValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Name is required.")
				.MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

			RuleFor(x => x.Position)
				.NotEmpty().WithMessage("Position is required.")
				.Must(position => _validPositions.Contains(position))
				.WithMessage($"Position must be one of the following: {string.Join(", ", _validPositions)}");

			RuleFor(x => x.DominantFoot)
				.Must(foot => string.IsNullOrEmpty(foot) || _validFeet.Contains(foot))
				.WithMessage($"Dominant foot must be one of the following: {string.Join(", ", _validFeet)}");

			RuleFor(x => x.PhoneNumber)
				.Must(BeAValidPhoneNumber)
				.When(x => !string.IsNullOrEmpty(x.PhoneNumber))
				.WithMessage("Phone number must be in a valid format.");

			RuleFor(x => x.ProfileDescription)
				.MaximumLength(1500).WithMessage("Bio cannot exceed 1500 characters.")
				.Must(NotContainEmailOrPhone)
				.WithMessage("Bio cannot contain email addresses or phone numbers.!!"); ;


			RuleFor(x => x.Weight)
				.GreaterThan(0).WithMessage("Weight must be greater than 0.")
				.LessThan(200).WithMessage("Weight must be less than 200 kg.");

			RuleFor(x => x.Height)
				.GreaterThan(0).WithMessage("Height must be greater than 0.")
				.LessThan(250).WithMessage("Height must be less than 250 cm.");

			RuleFor(x => x.CurrentTeam)
				.MaximumLength(100).WithMessage("Current team cannot exceed 100 characters.");
		}


		private bool NotContainEmailOrPhone(string description)
		{
			//in case of an empty bio return true
			if (string.IsNullOrEmpty(description))
				return true;

			// Regular expression for detecting email addresses
			var emailRegex = new Regex(@"[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+");

			// Regex for detecting Egyptian phone numbers (e.g., +201015374542 or 01015374542)
			var phoneRegex = new Regex(@"(?:(?:\+20)|0)1[0125]\d{8}");

			return !emailRegex.IsMatch(description) && !phoneRegex.IsMatch(description);
		}

		private bool BeAValidPhoneNumber(string phoneNumber)
		{
			if (string.IsNullOrEmpty(phoneNumber))
				return true;

			// Simple regex for international phone number format
			return Regex.IsMatch(phoneNumber, @"^\+?[0-9\s\-\(\)]{10,20}$");
		}
	}
}
