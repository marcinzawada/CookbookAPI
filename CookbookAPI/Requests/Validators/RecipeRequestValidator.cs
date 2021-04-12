using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookbookAPI.Data;
using CookbookAPI.Entities;
using CookbookAPI.Requests.Recipes;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CookbookAPI.Requests.Validators
{
    public class RecipeRequestValidator : AbstractValidator<RecipeRequest>
    {
        public RecipeRequestValidator(CookbookDbContext dbContext)
        {
            RuleFor(x => x.AreaId)
                .CustomAsync(async (value, context, cancellation) =>
                {
                    if (value == null)
                        return;

                    var areaExist = await dbContext.Areas.AnyAsync(x => x.Id == value);
                    if (!areaExist)
                        context.AddFailure("AreaId", "This area doesn't exist");
                });

            RuleFor(x => x.CategoryId)
                .CustomAsync(async (value, context, cancellation) =>
                {
                    if (value == null)
                        return;

                    var categoryExist = await dbContext.Categories.AnyAsync(x => x.Id == value);
                    if (!categoryExist)
                        context.AddFailure("CategoryId", "This category doesn't exist");
                });

            RuleFor(x => x.Ingredients)
                .NotEmpty()
                .CustomAsync(async (ingredients, context, cancellation) =>
                {
                    var ingredientIdsFromRequest = ingredients.Select(x => x.IngredientId);

                    var matchingIngredientIds = await dbContext.Ingredients.Where(
                        x => ingredientIdsFromRequest.Contains(x.Id)).Select(x => x.Id).ToListAsync();

                    if (matchingIngredientIds.Count == ingredients.Count)
                        return;

                    var noMatchingIds = ingredientIdsFromRequest.Where(x => !matchingIngredientIds.Contains(x)).ToArray();

                    if (noMatchingIds.Any())
                    {
                        context.AddFailure("Ingredients", 
                            $"Ingredients with id: {string.Join(" ", noMatchingIds)} don't exist");
                    }
                });
        }
    }
}
