using GloryScout.Domain.Dtos.IdentityDtos;
using GloryScout.Domain.Dtos.UserProfileDtos;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GloryScout.API.SwaggerSchemas
{
	public class AddSchemaDocumentFilter : IDocumentFilter
	{
		public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
		{
			context.SchemaGenerator.GenerateSchema(typeof(PlayerRegisterDto), context.SchemaRepository);
			context.SchemaGenerator.GenerateSchema(typeof(CoachRegisterDto), context.SchemaRepository);
			context.SchemaGenerator.GenerateSchema(typeof(UserDto), context.SchemaRepository);
			context.SchemaGenerator.GenerateSchema(typeof(SendResetCodeDto), context.SchemaRepository);
			context.SchemaGenerator.GenerateSchema(typeof(VerifyResetCodeDto), context.SchemaRepository);
			context.SchemaGenerator.GenerateSchema(typeof(CreatePostDto), context.SchemaRepository);
			context.SchemaGenerator.GenerateSchema(typeof(EditProfileDto), context.SchemaRepository);
			context.SchemaGenerator.GenerateSchema(typeof(PostDto), context.SchemaRepository);
			context.SchemaGenerator.GenerateSchema(typeof(UserProfileDto), context.SchemaRepository);

		}
	}
}
