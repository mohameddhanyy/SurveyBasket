using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Service;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using SurveyBasket.Api.Authentications;
using SurveyBasket.Api.Presistance;
using SurveyBasket.Api.Presistance.Models;
using SurveyBasket.Api.ServiceContracts;
using SurveyBasket.Api.Services;
using System.Reflection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SurveyBasket.Api.Helpers
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddDependancies(this IServiceCollection services)
        {
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // Add services
            services.AddScoped<IPollService, PollService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddSingleton<IJwtProvider, JwtProvider>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this#$is^secret*key")),
                    ValidIssuer = "SurveyBasket App",
                    ValidAudience = "SurveyBasket App Users"
                };

            });

            // Identity 
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<SurveyBasketDBContext>();
            // Add Mapster
            var mappingConfig = TypeAdapterConfig.GlobalSettings;
            mappingConfig.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton<IMapper>(new Mapper(mappingConfig));

            // Add Validations 
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
                .AddFluentValidationAutoValidation();

            return services;

        }
    }
}
