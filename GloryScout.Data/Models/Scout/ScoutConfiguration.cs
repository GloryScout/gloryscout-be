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

			


			// Relationships
			builder.HasOne(s => s.User)
				.WithMany()
				.HasForeignKey(s => s.UserId)
				.OnDelete(DeleteBehavior.Restrict);

		}
	}
}
