using FluentValidation;
using GloryScout.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Domain.Validators
{
	public class CreatePostDtoValidator : AbstractValidator<CreatePostDto>
	{
		private readonly string[] _allowedFileExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

		public CreatePostDtoValidator()
		{
			RuleFor(x => x.Description)
				.MaximumLength(1500).WithMessage("Description cannot exceed 1500 characters.");

			When(x => x.Image != null, () =>
			{
				RuleFor(x => x.Image.Length)
					.LessThanOrEqualTo(20 * 1024 * 1024).WithMessage("Image size must be less than 20MB.");

				RuleFor(x => x.Image.FileName)
					.Must(fileName => _allowedFileExtensions.Any(ext => fileName.EndsWith(ext)))
					.WithMessage($"Image must be one of the following formats: {string.Join(", ", _allowedFileExtensions)}");
			});
		}
	}

	public class UpdatePostDtoValidator : AbstractValidator<UpdatePostDto>
	{
		public UpdatePostDtoValidator()
		{
			//RuleFor(x => x.Id)
			//	.NotEmpty().WithMessage("Post ID is required.");

			RuleFor(x => x.Description)
				.MaximumLength(1500).WithMessage("Description cannot exceed 1500 characters.");
		}
	}
}
