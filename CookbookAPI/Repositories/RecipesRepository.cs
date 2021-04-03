using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookbookAPI.Data;
using CookbookAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CookbookAPI.Repositories
{
    public class RecipesRepository : BaseRepository<Recipe, CookbookDbContext>
    {
        public RecipesRepository(CookbookDbContext context) : base(context)
        {
        }

        public async Task<Recipe> GetRecipeWithDetails(int id)
        {
            var recipe = await _context.Recipes
                .Include(x => x.RecipeIngredients)
                .ThenInclude(x => x.Ingredient)
                .Include(x => x.Area)
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id);

            return recipe;
        }
    }
}
