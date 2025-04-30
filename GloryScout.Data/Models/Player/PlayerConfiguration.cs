using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloryScout.Data
{
	public class PlayerConfiguration : IEntityTypeConfiguration<Player>
	{
		public void Configure(EntityTypeBuilder<Player> builder)
		{
			builder.HasKey(p => p.Id);


			builder.Property(p => p.Position)
				.IsRequired()
				.HasMaxLength(50);

			builder.Property(p => p.DominantFoot)
				.HasMaxLength(20);


			

			builder.Property(p => p.CurrentTeam)
				.HasMaxLength(100);

			// Relationships
			builder.HasOne(p => p.User)
				.WithMany()
				.HasForeignKey(p => p.UserId)
				.OnDelete(DeleteBehavior.Restrict);

			
		}
	}
}
