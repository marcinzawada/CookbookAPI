using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookbookAPI.ApiClients;
using CookbookAPI.ApiClients.Interfaces;
using CookbookAPI.Authorization;
using CookbookAPI.Common;
using CookbookAPI.Common.Interfaces;
using CookbookAPI.DTOs.MealDB;
using CookbookAPI.Entities;
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;

namespace CookbookAPI.Extensions.StartupInstallers
{
    public static class DependencyInjectionInstaller
    {
        public static void InstallDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMealApiClient, MealApiClient>();
            services.AddScoped<IApiClient, ApiClient>();
            services.AddTransient<IRestClient, RestClient>();
            services.AddScoped<IDtoToEntityMapper<MealRecipeDto, Recipe>, MealRecipeDtoToRecipeMapper>();
            services.AddScoped<ISeeder, MealDbSeeder>();
            services.AddHttpContextAccessor();
            services.AddScoped<RequestTimeMiddleware>();
            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddScoped<IJwtGenerator, JwtGenerator>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            services.AddScoped<IUserContextService, UserContextService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IRecipesService, RecipesService>();
            services.AddScoped<IIngredientsService, IngredientsService>();

            services.AddScoped<IUserRepository<User>, UserRepository>();
            services.AddScoped<IRecipesRepository<Recipe>, RecipesRepository>();
            services.AddScoped<IIngredientsRepository<Ingredient>, IngredientsRepository>();
            
            services.AddScoped<IAuthorizationHandler, RecipeOperationHandler>();
            services.AddScoped<IAuthorizationHandler, IngredientOperationHandler>();

        }
    }
}
