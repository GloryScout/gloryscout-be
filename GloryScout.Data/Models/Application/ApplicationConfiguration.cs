using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Data
{
	public class ApplicationConfiguration : IEntityTypeConfiguration<Application>
	{
		public void Configure(EntityTypeBuilder<Application> builder)
		{
			builder.HasKey(a => a.Id);

			builder.Property(a => a.Status)
				.HasMaxLength(50);

			builder.Property(a => a.Content)
				.HasMaxLength(2000);

			builder.Property(a => a.SubmittedAt)
				.IsRequired()
				.HasDefaultValueSql("GETDATE()");

			// Relationships
			builder.HasOne(a => a.Scout)
				.WithMany(s => s.Applications)
				.HasForeignKey(a => a.ScoutId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(a => a.Player)
				.WithMany(p => p.Applications)
				.HasForeignKey(a => a.PlayerId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
