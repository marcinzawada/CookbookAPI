using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CookbookAPI.Entities;
using Microsoft.AspNetCore.Authorization;

namespace CookbookAPI.Authorization
{
    public class RecipeOperationHandler : AuthorizationHandler<RecipeOperationRequirement, Recipe>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RecipeOperationRequirement requirement,
            Recipe recipe)
        {
            if (requirement.ResourceOperation == ResourceOperation.Read ||
                requirement.ResourceOperation == ResourceOperation.Create)
            {
                context.Succeed(requirement);
            }

            var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (recipe.UserId == int.Parse(userId))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
