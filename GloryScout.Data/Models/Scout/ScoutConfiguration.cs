using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Data
{
	public class ScoutConfiguration : IEntityTypeConfiguration<Scout>
	{
		public void Configure(EntityTypeBuilder<Scout> builder)
		{
			builder.HasKey(s => s.Id);

			builder.Property(s => s.ClubName)
				.IsRequired()
				.HasMaxLength(100);

			builder.Property(s => s.ProfileDescription)
				.HasMaxLength(1000);

			builder.Property(s => s.ContactDetails)
				.IsRequired()
				.HasMaxLength(500);

			builder.Property(s => s.Location)
				.IsRequired()
				.HasMaxLength(100);

			// Relationships
			builder.HasOne(s => s.User)
				.WithMany()
				.HasForeignKey(s => s.UserId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasMany(s => s.Applications)
				.WithOne(a => a.Scout)
				.HasForeignKey(a => a.ScoutId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
