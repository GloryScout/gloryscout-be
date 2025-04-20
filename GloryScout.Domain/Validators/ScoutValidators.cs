//using FluentValidation;
//using GloryScout.Domain.Dtos;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace GloryScout.Domain.Validators
//{
//	public class CreateScoutProfileDtoValidator : AbstractValidator<CreateScoutProfileDto>
//	{
//		public CreateScoutProfileDtoValidator()
//		{
//			RuleFor(x => x.ClubName)
//				.NotEmpty().WithMessage("Club name is required.")
//				.MaximumLength(100).WithMessage("Club name cannot exceed 100 characters.");

//			RuleFor(x => x.ProfileDescription)
//				.MaximumLength(1000).WithMessage("Profile description cannot exceed 1000 characters.");

//			RuleFor(x => x.ContactDetails)
//				.NotEmpty().WithMessage("Contact details are required.")
//				.MaximumLength(500).WithMessage("Contact details cannot exceed 500 characters.");

//			RuleFor(x => x.Location)
//				.NotEmpty().WithMessage("Location is required.")
//				.MaximumLength(100).WithMessage("Location cannot exceed 100 characters.");
//		}
//	}

//	public class UpdateScoutProfileDtoValidator : AbstractValidator<UpdateScoutProfileDto>
//	{
//		public UpdateScoutProfileDtoValidator()
//		{
//			RuleFor(x => x.ClubName)
//				.NotEmpty().WithMessage("Club name is required.")
//				.MaximumLength(100).WithMessage("Club name cannot exceed 100 characters.");

//			RuleFor(x => x.ProfileDescription)
//				.MaximumLength(1000).WithMessage("Profile description cannot exceed 1000 characters.");

//			RuleFor(x => x.ContactDetails)
//				.NotEmpty().WithMessage("Contact details are required.")
//				.MaximumLength(500).WithMessage("Contact details cannot exceed 500 characters.");

//			RuleFor(x => x.Location)
//				.NotEmpty().WithMessage("Location is required.")
//				.MaximumLength(100).WithMessage("Location cannot exceed 100 characters.");
//		}
//	}
//}
