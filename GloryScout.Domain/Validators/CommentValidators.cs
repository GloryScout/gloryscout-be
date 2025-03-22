using FluentValidation;
using GloryScout.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Domain.Validators
{
	public class CreateCommentDtoValidator : AbstractValidator<CreateCommentDto>
	{
		public CreateCommentDtoValidator()
		{
			
			RuleFor(x => x.CommentedText)
				.NotEmpty().WithMessage("Comment text is required.")
				.MaximumLength(500).WithMessage("Comment text cannot exceed 500 characters.");
		}
	}

	public class UpdateCommentDtoValidator : AbstractValidator<UpdateCommentDto>
	{
		public UpdateCommentDtoValidator()
		{
			
			RuleFor(x => x.CommentedText)
				.NotEmpty().WithMessage("Comment text is required.")
				.MaximumLength(500).WithMessage("Comment text cannot exceed 500 characters.");
		}
	}
}
