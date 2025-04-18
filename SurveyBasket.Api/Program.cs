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


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseCustomMiddleware();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

//app.UseOutputCache();


app.MapControllers();

app.UseExceptionHandler();

app.Run();
