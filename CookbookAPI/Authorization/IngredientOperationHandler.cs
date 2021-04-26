using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CookbookAPI.Data;
using CookbookAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CookbookAPI.Authorization
{
    public class IngredientOperationHandler : AuthorizationHandler<IngredientOperationRequirement, Ingredient>
    {
        private readonly CookbookDbContext _context;

        public IngredientOperationHandler(CookbookDbContext context)
        {
            _context = context;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IngredientOperationRequirement requirement,
            Ingredient ingredient)
        {
            if (requirement.ResourceOperation == ResourceOperation.Read ||
                requirement.ResourceOperation == ResourceOperation.Create)
            {
                context.Succeed(requirement);
            }

            var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (ingredient.UserId == int.Parse(userId))
            {
                var ingredientIsUseByAnotherUser = await _context.Recipes.AsNoTracking()
                    .Include(r => r.RecipeIngredients)
                    .Where(r => 
                        r.UserId != ingredient.UserId &&
                        r.RecipeIngredients.Any(ri => 
                            ri.IngredientId == ingredient.Id))
                    .AnyAsync();

                if (!ingredientIsUseByAnotherUser)
                    context.Succeed(requirement);
            }

            await Task.CompletedTask;
        }
    }
}
