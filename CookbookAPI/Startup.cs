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
using CookbookAPI.Extensions.StartupInstallers;
using CookbookAPI.Mappers;
using CookbookAPI.Mappers.Interfaces;
using CookbookAPI.Middleware;
using CookbookAPI.Repositories;
using CookbookAPI.Repositories.Interfaces;
using CookbookAPI.Requests.Account;
using CookbookAPI.Requests.Ingredients;
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
            services.AddControllers()
                .AddFluentValidation(opt => opt.RegisterValidatorsFromAssembly(this.GetType().Assembly));

            services.InstallAuthentication(Configuration);

            services.AddAuthorization();

            services.InstallSwagger(Configuration);

            var connectionString = Configuration.GetConnectionString("CookbookDbConnection");
            services.AddDbContext<CookbookDbContext>(x => x.UseSqlServer(connectionString));

            services.AddAutoMapper(this.GetType().Assembly);
            services.AddCors();

            services.InstallDependencyInjection(Configuration);
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
