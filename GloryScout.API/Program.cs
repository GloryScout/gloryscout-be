using System.Reflection;
using FluentValidation.AspNetCore;
using GloryScout.API.Services;
using GloryScout.API.Services.PlayerServiceandCoach;
using GloryScout.Services;





#pragma warning disable CS0618

var builder = WebApplication.CreateBuilder(args);

#region services & DI


// Registers Application Services , Passes Configuration Settings and Facilitates Dependency Injection
builder.Services.AddApplicationServices(builder.Configuration);



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policyBuilder =>
        {
            policyBuilder
				.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
});
});
builder.Services.AddControllers()
	.AddFluentValidation(options =>
	{
		// Validate child properties and root collection elements
		options.ImplicitlyValidateChildProperties = true;
		options.ImplicitlyValidateRootCollectionElements = true;

		// Automatic registration of validators in assembly
		options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
	})
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
	});
       builder.Services.AddScoped<IPlayerService, PlayerService>();

//builder.WebHost
//	   .UseKestrel()
//	   .UseUrls("http://0.0.0.0:5000", "https://0.0.0.0:5001");

#endregion

#region pipeline

var app = builder.Build();


//using (var scope = app.Services.CreateScope())
//{
//	var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//	SeedData.Seed(dbContext);
//}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	//app.UseDeveloperExceptionPage();
	app.UseSwagger();
	app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Spare Parts v1"));
}
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Spare Parts v1"));



//app.Use(async (context, next) =>
//{
//    Counter++;
//    await next(context);
//});

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();



//Data Source=DESKTOP-8BMN06A;Initial Catalog=GloryScoutDatabase;Integrated Security=True
#endregion

#region Program

partial class Program
{
	//public static int Counter = 0;
}

#endregion

