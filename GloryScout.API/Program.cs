using System.Reflection;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using GloryScout.API.Services;
using GloryScout.API.Services;
using X.Paymob.CashIn;

#pragma warning disable CS0618

var builder = WebApplication.CreateBuilder(args);

#region services & DI

// Registers Application Services , Passes Configuration Settings and Facilitates Dependency Injection
builder.Services.AddApplicationServices(builder.Configuration);

//builder.Services.AddScoped<IOrderService, OrderService>();

var configuration = builder.Configuration;

//paymob

//builder.Services.AddHttpClient<PaymobService>();
//builder.Services.AddPaymobCashIn(config => {
//	var paymobSection = configuration.GetSection("Paymob");

//	// Fix for CS8601: Possible null reference assignment.
//	builder.Services.AddPaymobCashIn(cashInConfig =>
//	{
//		var paymobSection = configuration.GetSection("Paymob");

//		// Use null-coalescing operator to provide a default value or throw an exception if null
//		cashInConfig.ApiKey = paymobSection["ApiKey"] ?? throw new InvalidOperationException("Paymob ApiKey is not configured.");
//		cashInConfig.Hmac = paymobSection["SecretKey"] ?? throw new InvalidOperationException("Paymob SecretKey is not configured.");
//	});

//});


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

#endregion

#region pipeline

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GloryScout v1"));
}

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

#endregion

#region Program

partial class Program
{
    //public static int Counter = 0;
}

#endregion
