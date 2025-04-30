using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using GloryScout.Data.Models.Followers;


namespace GloryScout.Data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid, IdentityUserClaim<Guid>, IdentityUserRole<Guid>, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        #region constructors
        public AppDbContext()
        {
        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        #endregion

        #region DbSet

        public override DbSet<User> Users => Set<User>();
        public virtual DbSet<ResetPassword> ResetPasswords => Set<ResetPassword>();
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Like> Likes => Set<Like>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<VerificationCode> VerificationCodes => Set<VerificationCode>();
        public DbSet<Player> Players => Set<Player>();
        public DbSet<Scout> Scouts => Set<Scout>();
		public DbSet<UserFollowings> UserFollowings => Set<UserFollowings>();


		#endregion

        #region OnConfiguration

		// add anything related to on configuatoion iof the db 
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-8BMN06A;Initial Catalog=GloryScoutDatabase;Integrated Security=True; TrustServerCertificate=True;");
        }
        #endregion

        #region OnModelCreating

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply entity configurations from assemblies
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PlayerConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LikeConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(VerificationCodeConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ScoutConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CommentConfiguration).Assembly);

            // Seed identity roles
            modelBuilder.Entity<IdentityRole<Guid>>().HasData(
                new IdentityRole<Guid>
                {
                    Id = Guid.Parse("A8D3C1E1-BCC3-4B3E-AB7C-A7F7FBD27231"),
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "A8D3C1E1-BCC3-4B3E-AB7C-A7F7FBD27231"
                },
                new IdentityRole<Guid>
                {
                    Id = Guid.Parse("27D0E2E2-40E0-4CF2-8267-19F1AC77D53B"),
                    Name = "Player",
                    NormalizedName = "PLAYER",
                    ConcurrencyStamp = "27D0E2E2-40E0-4CF2-8267-19F1AC77D53B"
                },
                new IdentityRole<Guid>
                {
                    Id = Guid.Parse("E3F1286B-79D2-46C3-98E4-91F89E10E93D"),
                    Name = "Scout",
                    NormalizedName = "SCOUT",
                    ConcurrencyStamp = "E3F1286B-79D2-46C3-98E4-91F89E10E93D"
                }
            );


            // Change Identity Schema and Table Names
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<IdentityRole<Guid>>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UserRole");
            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");


			modelBuilder.Entity<UserFollowings>()
		.HasKey(uf => new { uf.FollowerId, uf.FolloweeId }); // Composite primary key

			modelBuilder.Entity<UserFollowings>()
				.HasOne(uf => uf.Follower)
				.WithMany(u => u.Following)
				.HasForeignKey(uf => uf.FollowerId)
				.OnDelete(DeleteBehavior.Restrict); // Prevent cascade deletes

			modelBuilder.Entity<UserFollowings>()
				.HasOne(uf => uf.Followee)
				.WithMany(u => u.Followers)
				.HasForeignKey(uf => uf.FolloweeId)
				.OnDelete(DeleteBehavior.Restrict); // Prevent cascade deletes
		}
	}
    
}
        
#endregion