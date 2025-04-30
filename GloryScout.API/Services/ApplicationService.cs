using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using GloryScout.Data;
using GloryScout.API.Services.Auth;
using GloryScout.API.Services.Token;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AutoMapper;
using Microsoft.Win32;
using GloryScout.Domain.Profiles;
using GloryScout.Data.Repository;
using GloryScout.Data.Repository.UesrRepo;
using GloryScout.Data.Repository.PlayerRepo;
using GloryScout.Data.Repository.ScoutRepo;
using SpareParts.Data;
using GloryScout.API.Services.UserProfiles;



namespace GloryScout.API.Services;

public static class ApplicationService
{

	//adding servicess to the contaoner
	//static extension method to encapsulate all your service registrations in one place
	public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
	{
		#region Database

		var connectionString = config.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

		services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

		#endregion

		#region Identity Managers
		services.AddIdentity<User, IdentityRole<Guid>>(options =>
		{
			options.Password.RequireNonAlphanumeric = true;
			options.Password.RequireLowercase = false;
			options.Password.RequireUppercase = false;
			options.Password.RequiredLength = 6;

			options.User.RequireUniqueEmail = true;

			options.Lockout.MaxFailedAccessAttempts = 3;
			options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
		})
			.AddEntityFrameworkStores<AppDbContext>()
			.AddDefaultTokenProviders();


		#endregion

		#region JWT & Authentication

		services.AddScoped<IAuthService, AuthService>();

		services.Configure<Jwt>(config.GetSection("JWT"));
		services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		}).AddJwtBearer(o =>
		{
			o.RequireHttpsMetadata = false;
			o.SaveToken = false;
			o.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				ValidIssuer = config["JWT:Issuer"],
				ValidAudience = config["JWT:Audience"],
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]!))
			};
		});

		#endregion

		#region Authorization

		services.AddAuthorization(options =>
		{
			options.AddPolicy("Admin",
				policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
			options.AddPolicy("Player",
				policy => policy.RequireClaim(ClaimTypes.Role, "Admin","Player"));
			options.AddPolicy("Scout",
				policy => policy.RequireClaim(ClaimTypes.Role, "Admin","Scout"));
			//options.AddPolicy("User",
			//	policy => policy.RequireClaim(ClaimTypes.Role, "Admin", "Manager", "Client", "User"));

		}
		);

		#endregion

		#region Swagger

		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen(c =>
		{
			c.SwaggerDoc("v1", new() { Title = "Glory scout API", Version = "v1" });
			var jwtSecurityScheme = new OpenApiSecurityScheme
			{
				Scheme = "bearer",
				BearerFormat = "JWT",
				Name = "JWT Authentication",
				In = ParameterLocation.Header,
				Type = SecuritySchemeType.Http,
				Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

				Reference = new OpenApiReference
				{
					Id = JwtBearerDefaults.AuthenticationScheme,
					Type = ReferenceType.SecurityScheme
				}
			};

			c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

			c.AddSecurityRequirement(new OpenApiSecurityRequirement
		{
			{jwtSecurityScheme, Array.Empty<string>()}
		});

		});



		#endregion
		#region Repos & AutoMapper

		//Register AutoMapper with the ApplicationProfile
		services.AddAutoMapper(typeof(ApplicationProfile).Assembly);

		// Register unit of work pattern components
		services.AddScoped<IUnitOfWork, UnitOfWork>();

		// Register repositories
		services.AddScoped<IUserRepo, UserRepo>();
		services.AddScoped<IPlayerRepo, PlayerRepo>();
		services.AddScoped<IScoutRepo, ScoutRepo>();
		//services.AddScoped<IApplicationRepo, ApplicationRepo>();
		//services.AddScoped<IPostRepo, PostRepo>();
		//services.AddScoped<ICommentRepo, CommentRepo>();
		//services.AddScoped<ILikeRepo, LikeRepo>();
		//services.AddScoped<IVerificationCodeRepo, VerificationCodeRepo>();

		//// Register business logic managers
		//services.AddScoped<IUserManager, UserManager>();
		//services.AddScoped<IPlayerManager, PlayerManager>();
		//services.AddScoped<IScoutManager, ScoutManager>();
		//services.AddScoped<IApplicationManager, ApplicationManager>();
		//services.AddScoped<IPostManager, PostManager>();
		//services.AddScoped<IAuthManager, AuthManager>();
		//services.AddScoped<IProfileManager, ProfileManager>();

		#endregion

		#region Cloudinary 

		services.AddSingleton<CloudinaryService>();

		#endregion

		#region other services

		services.AddScoped<IUserProfileService, UserProfileService>();

		#endregion

		return services;
	}
}
