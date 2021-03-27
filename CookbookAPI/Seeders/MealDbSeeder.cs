using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CookbookAPI.ApiClients.Interfaces;
using CookbookAPI.Data;
using CookbookAPI.DTOs.MealDB;
using CookbookAPI.Entities;
using CookbookAPI.Mappers;
using CookbookAPI.Mappers.Interfaces;
using CookbookAPI.Seeders.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CookbookAPI.Seeders
{
    public class MealDbSeeder : ISeeder
    {
        private readonly CookbookDbContext _context;
        private readonly IMealApiClient _client;
        private readonly IMapper _autoMapper;
        private readonly IDtoToEntityMapper<MealRecipeDto, Recipe> _recipeMapper;

        public MealDbSeeder(CookbookDbContext context, IMealApiClient client,
            IMapper autoMapper, IDtoToEntityMapper<MealRecipeDto, Recipe> recipeMapper )
        {
            _context = context;
            _client = client;
            _autoMapper = autoMapper;
            _recipeMapper = recipeMapper;
        }

        public async Task Seed()
        {
            if (await _context.Database.CanConnectAsync())
            {
                var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
                if (pendingMigrations != null && pendingMigrations.Any())
                {
                    await _context.Database.MigrateAsync();
                }

                if (! await _context.Categories.AnyAsync())
                {
                    var categories = await GetCategories();
                    await _context.Categories.AddRangeAsync(categories);
                    await _context.SaveChangesAsync();
                }

                if (! await _context.Areas.AnyAsync())
                {
                    var areas = await GetAreas();
                    await _context.Areas.AddRangeAsync(areas);
                    await _context.SaveChangesAsync();
                }

                if (! await _context.Ingredients.AnyAsync())
                {
                    var ingredients = await GetIngredients();
                    await _context.Ingredients.AddRangeAsync(ingredients);
                    await _context.SaveChangesAsync();
                }

                if (! await _context.Recipes.AnyAsync())
                {
                    var recipes = await GetRecipes();
                    await _context.Recipes.AddRangeAsync(recipes);
                    await _context.SaveChangesAsync();
                }
            }
        }

        private async Task<List<Category>> GetCategories()
        {
            var categoryDtos = await _client.GetAllMealCategories();
            var categories = _autoMapper.Map<List<Category>>(categoryDtos);
            return categories;
        }

        private async Task<List<Area>> GetAreas()
        {
            var areaDtos = await _client.GetAllMealAreas();
            var areas = _autoMapper.Map<List<Area>>(areaDtos);
            return areas;
        }

        private async Task<List<Ingredient>> GetIngredients()
        {
            var ingredientDtos = await _client.GetAllMealIngredients();
            var ingredients = _autoMapper.Map<List<Ingredient>>(ingredientDtos);
            return ingredients;
        }

        private async Task<List<Recipe>> GetRecipes()
        {
            var recipeDtos = await _client.GetAllMealRecipes();
            var recipes = await _recipeMapper.MapManyAsync(recipeDtos);
            return recipes;
        }
    }
}
