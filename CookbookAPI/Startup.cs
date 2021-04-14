using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookbookAPI.ApiClients;
using CookbookAPI.ApiClients.Interfaces;
using CookbookAPI.Authorization;
using CookbookAPI.Common;
using CookbookAPI.Common.Interfaces;
using CookbookAPI.Data;
using CookbookAPI.DTOs.MealDB;
using CookbookAPI.Entities;
using CookbookAPI.Mappers;
using CookbookAPI.Mappers.Interfaces;
using CookbookAPI.Middleware;
using CookbookAPI.Repositories;
using CookbookAPI.Requests.Account;
using CookbookAPI.Requests.Recipes;
using CookbookAPI.Requests.Validators;
using CookbookAPI.Seeders;
using CookbookAPI.Seeders.Interfaces;
using CookbookAPI.Services;
using CookbookAPI.Services.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using RestSharp;

namespace CookbookAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddFluentValidation();

            var authenticationSettings = new AuthenticationSettings();

            Configuration.GetSection("Authentication").Bind(authenticationSettings);
            services.AddSingleton(authenticationSettings);

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = "Bearer";
                option.DefaultScheme = "Bearer";
                option.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
                };
            });

            services.AddAuthorization();

            services.AddSwaggerDocument(document =>
            {
                document.Title = "Cookbook API Documentation";
                document.DocumentName = "swagger";
                document.OperationProcessors.Add(new OperationSecurityScopeProcessor("jwt"));
                document.DocumentProcessors.Add(new SecurityDefinitionAppender("jwt", new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "JWT Token - remember to add 'Bearer ' before the token",
                }));
            });

            var connectionString = Configuration.GetConnectionString("LocalDb");
            services.AddDbContext<CookbookDbContext>(x => x.UseSqlServer(connectionString));

            services.AddAutoMapper(this.GetType().Assembly);
            services.AddScoped<IMealApiClient, MealApiClient>();
            services.AddScoped<IApiClient,ApiClient>();
            services.AddTransient<IRestClient, RestClient>();
            services.AddScoped<IDtoToEntityMapper<MealRecipeDto, Recipe>, MealRecipeDtoToRecipeMapper>();
            services.AddScoped<ISeeder, MealDbSeeder>();
            services.AddHttpContextAccessor();
            services.AddCors();
            services.AddScoped<RequestTimeMiddleware>();
            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<RecipesService>();
            services.AddScoped<UserRepository>();
            services.AddScoped<RecipesRepository>();
            services.AddScoped<IJwtGenerator, JwtGenerator>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>();
            services.AddScoped<IValidator<GetRecipesRequest>, GetRecipesRequestValidator>();
            services.AddScoped<IValidator<RecipeRequest>, RecipeRequestValidator>();
            services.AddScoped<IAuthorizationHandler, RecipeOperationHandler>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ISeeder seeder, CookbookDbContext context)
        {
            app.UseCors(builder =>
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
            );

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseMiddleware<RequestTimeMiddleware>();

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseOpenApi(options =>
            {
                options.DocumentName = "swagger";
                options.Path = "/swagger/v1/swagger.json";
                options.PostProcess = (document, _) =>
                {
                    document.Schemes.Add(OpenApiSchema.Https);
                };
            });

            app.UseSwaggerUi3(options =>
            {
                options.DocumentPath = "/swagger/v1/swagger.json";
            });


            if (context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                ConfigureAsync(seeder).Wait();
            }
            
        }

        public async Task ConfigureAsync(ISeeder seeder)
        {
            await seeder.Seed().ConfigureAwait(false);
        }
    }
}
