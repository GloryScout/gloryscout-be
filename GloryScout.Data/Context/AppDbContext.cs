using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using GloryScout.Data.Models.Entities;
using GloryScout.Domain.Entities;


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
        public DbSet<Application> Applications => Set<Application>();
        public DbSet<Player> Players => Set<Player>();
        public DbSet<Scout> Scouts => Set<Scout>();
        public DbSet<PlayerProfile> PlayerProfiles { get; set; }
        public DbSet<Media> MediaItems { get; set; }
        public DbSet<CoachProfile> CoachProfiles { get; set; }


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
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationConfiguration).Assembly);
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

            // Configure relationships for PlayerProfile and Media
            modelBuilder.Entity<PlayerProfile>()
                .HasMany(p => p.MediaItems)
                .WithOne(m => m.PlayerProfile)
                .HasForeignKey(m => m.PlayerProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure relationships for CoachProfile and Media
            modelBuilder.Entity<CoachProfile>()
                .HasMany(c => c.MediaItems)
                .WithOne(m => m.CoachProfile)
                .HasForeignKey(m => m.CoachProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            // Change Identity Schema and Table Names
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<IdentityRole<Guid>>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UserRole");
            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");
        }
    }
}
        
#endregion