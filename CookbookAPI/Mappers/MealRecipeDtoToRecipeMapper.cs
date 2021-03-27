using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CookbookAPI.Data;
using CookbookAPI.DTOs.MealDB;
using CookbookAPI.Entities;
using CookbookAPI.Mappers.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CookbookAPI.Mappers
{
    public class MealRecipeDtoToRecipeMapper : IDtoToEntityMapper<MealRecipeDto, Recipe>
    {
        private readonly CookbookDbContext _context;
        private readonly ILogger<MealRecipeDtoToRecipeMapper> _logger;

        public MealRecipeDtoToRecipeMapper(CookbookDbContext context, ILogger<MealRecipeDtoToRecipeMapper> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Recipe> MapAsync(MealRecipeDto dto)
        {
            var ingredientProps = dto.GetType().GetProperties().Where(x => x.Name.Contains("Ingredient"));
            var measureProps = dto.GetType().GetProperties().Where(x => x.Name.Contains("Measure"));

            var ingredientsFromDto = ingredientProps.Select(ingredientProp => ingredientProp.GetValue(dto)?.ToString()).ToList();
            var measureFromDto = measureProps.Select(measureProp => measureProp.GetValue(dto)?.ToString()).ToList();

            var ingredients = await _context.Ingredients.Where(x => ingredientsFromDto.Contains(x.Name)).ToListAsync();

            if ((ingredientsFromDto.Count() != measureFromDto.Count()) && ingredientsFromDto.Count() != ingredients.Count())
            {
                _logger.LogError("Ingredients from api aren't in base");
                throw new Exception("Ingredients from api aren't in base");
            }

            var category = _context.Categories.FirstOrDefault(x => x.Name.ToLower() == dto.Category.ToLower());

            if (category is null)
            {
                _logger.LogError($"Category: {dto.Category} not found in database");
                throw new Exception($"Category: {dto.Category} not found in database");
            }

            var area = _context.Areas.FirstOrDefault(x => x.Name.ToLower() == dto.Area.ToLower());

            if (area is null)
            {
                _logger.LogError($"Area: {dto.Area} not found in database");
                throw new Exception($"Area: {dto.Area} not found in database");
            }

            var recipe = new Recipe
            {
                Name = dto.Name,
                Category = category,
                Area = area,
                Instructions = dto.Instructions,
                Thumbnail = dto.Thumbnail,
                Youtube = dto.Youtube,
                Source = dto.Source
            };

            for (var i = 0; i < ingredients.Count; i++)
            {
                recipe.RecipeIngredients.Add(new RecipeIngredient
                {
                    Recipe = recipe,
                    Ingredient = ingredients[i],
                    Measure = measureFromDto[i]
                });
            }

            return recipe;
        }

        public async Task<List<Recipe>> MapManyAsync(List<MealRecipeDto> dtos)
        {
            var recipes = new List<Recipe>();

            foreach (var dto in dtos)
            {
                recipes.Add(await MapAsync(dto));
            }

            return recipes;
        }
    }
}
