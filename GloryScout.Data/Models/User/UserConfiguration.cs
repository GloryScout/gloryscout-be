using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Data
{
	public class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.HasKey(u => u.Id);
			builder.Property(u => u.UserName)
				.IsRequired();

			builder.Property(u => u.Email)
				.IsRequired()
				.HasMaxLength(256);

			builder.Property(u => u.Nationality)
				.IsRequired()
				.HasMaxLength(100);

			builder.Property(u => u.UserType)
				.IsRequired()
				.HasMaxLength(10);

			builder.Property(u => u.CreatedAt)
				.IsRequired()
				.HasDefaultValueSql("GETDATE()");

			builder.Property(u => u.IsVerified)
				.IsRequired()
				.HasDefaultValue(false);

			// Relationships
			builder.HasMany(u => u.Posts)
				.WithOne(p => p.User)
				.HasForeignKey(p => p.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(u => u.VerificationCodes)
				.WithOne(v => v.User)
				.HasForeignKey(v => v.UserId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
