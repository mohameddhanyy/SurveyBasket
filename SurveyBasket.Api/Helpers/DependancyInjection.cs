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
        public static IServiceCollection AddDependancies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // Add Cors
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();

                });
            }); 
            // Add services
            services.AddScoped<IPollService, PollService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IVoteService, VoteService>();
            services.AddScoped<IResultService, ResultService>();
            services.AddSingleton<IJwtProvider, JwtProvider>();

            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            // Identity 
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<SurveyBasketDBContext>();

            // Add Options

            //services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
            services.AddOptions<JwtOptions>()
                .BindConfiguration(nameof(JwtOptions))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

            // Jwt
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions?.Key!)),
                    ValidIssuer = jwtOptions?.Issuer,
                    ValidAudience = jwtOptions?.Audience
                };
            });


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
