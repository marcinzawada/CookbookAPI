using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookbookAPI.Data;
using CookbookAPI.Entities;
using CookbookAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CookbookAPI.Repositories
{
    public class RecipesRepository : BaseRepository<Recipe, CookbookDbContext>, IRecipesRepository<Recipe>
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

        public async Task<Recipe> GetWithRecipeIngredients(int id)
        {
            var recipe = await _context.Recipes
                .Include(x => x.RecipeIngredients)
                .FirstOrDefaultAsync(x => x.Id == id);

            return recipe;
        }
    }
}
