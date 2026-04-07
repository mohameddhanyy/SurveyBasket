using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Service;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using SurveyBasket.Api.Helpers;
using SurveyBasket.Api.Middlewares;
using SurveyBasket.Api.Presistance;
using SurveyBasket.Api.Presistance.Models;
using System.Reflection;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDependancies(builder.Configuration);

// Output Cache
///builder.Services.AddOutputCache(options =>
///{
///    options.AddPolicy("Polls", x =>
///    x.Cache()
///    .Expire(TimeSpan.FromSeconds(120))
///    .Tag("Available"));
///});

// In-Memory Cache
builder.Services.AddMemoryCache();

// Distributed cache
builder.Services.AddDistributedMemoryCache();

builder.Host.UseSerilog((context, confiuration) =>
    confiuration.ReadFrom.Configuration(context.Configuration)
);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SurveyBasketDBContext>(options =>
{
    options.UseSqlServer(connectionString);
});
// Add services to the container.

//add azure key vault configuration
var keyVaultUrl = builder.Configuration["KeyVault:VaultUrl"];
if (!string.IsNullOrEmpty(keyVaultUrl))
{
    try
    {
        builder.Configuration.AddAzureKeyVault(
            new Uri(keyVaultUrl),
            new Azure.Identity.DefaultAzureCredential()
        );
    }
    catch (Exception ex)
    {
        // Log the error but don't fail the application startup
        Console.WriteLine($"Warning: Failed to configure Azure Key Vault: {ex.Message}");
    }
}
builder.Services.AddApplicationInsightsTelemetry(new Microsoft.ApplicationInsights.AspNetCore.Extensions.ApplicationInsightsServiceOptions
{
    ConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    
    options.RoutePrefix = string.Empty; 
});

app.UseSerilogRequestLogging();

app.UseCustomMiddleware();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

//app.UseOutputCache();


app.MapControllers();

app.UseExceptionHandler();

app.Run();
