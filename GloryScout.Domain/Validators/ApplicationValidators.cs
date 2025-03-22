using FluentValidation;
using GloryScout.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Domain.Validators
{
	public class CreateApplicationDtoValidator : AbstractValidator<CreateApplicationDto>
	{
		public CreateApplicationDtoValidator()
		{
		
			RuleFor(x => x.Content)
				.NotEmpty().WithMessage("Application content is required.")
				.MaximumLength(2000).WithMessage("Application content cannot exceed 2000 characters.")
				.MinimumLength(100).WithMessage("Application content cannot be less than 200 characters.");
		}
	}

	public class UpdateApplicationStatusDtoValidator : AbstractValidator<UpdateApplicationContentDto>
	{
		
		public UpdateApplicationStatusDtoValidator()
		{
			
			RuleFor(x => x.Content)
				.NotEmpty().WithMessage("Application content is required.")
				.MaximumLength(2000).WithMessage("Application content cannot exceed 2000 characters.")
				.MinimumLength(100).WithMessage("Application content cannot be less than 200 characters.");
		}
	}
}
