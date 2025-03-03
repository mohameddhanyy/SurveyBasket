using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Service;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using SurveyBasket.Api.Helpers;
using SurveyBasket.Api.Middlewares;
using SurveyBasket.Api.Presistance;
using SurveyBasket.Api.Presistance.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SurveyBasketDBContext>(options =>
{
    options.UseSqlServer(connectionString);
});
var configurations = builder.Configuration;
// Add services to the container.

builder.Services.AddDependancies(configurations);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
