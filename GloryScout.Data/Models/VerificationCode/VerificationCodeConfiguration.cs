using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Data
{
	public class VerificationCodeConfiguration : IEntityTypeConfiguration<VerificationCode>
	{
		public void Configure(EntityTypeBuilder<VerificationCode> builder)
		{
			builder.HasKey(v => v.Id);

			builder.Property(v => v.Code)
				.IsRequired()
				.HasMaxLength(10);

			builder.Property(v => v.UserEmail)
				.IsRequired()
				.HasMaxLength(256);

			builder.Property(v => v.IsUsed)
				.IsRequired()
				.HasDefaultValue(false);

			builder.Property(v => v.ExpiresAt)
				.IsRequired();

			// Relationships
			builder.HasOne(v => v.User)
				.WithMany(u => u.VerificationCodes)
				.HasForeignKey(v => v.UserId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
