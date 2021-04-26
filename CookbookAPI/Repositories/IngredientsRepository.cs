using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CookbookAPI.Data;
using CookbookAPI.Entities;
using CookbookAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CookbookAPI.Repositories
{
    public class IngredientsRepository : BaseRepository<Ingredient, CookbookDbContext>,
        IIngredientsRepository<Ingredient>
    {
        public IngredientsRepository(CookbookDbContext context) : base(context)
        {
        }

        public async Task<Ingredient> GetByIdWithRecipes(int id)
        {
            return await _context.Ingredients
                .Include(x => x.RecipeIngredient)
                .ThenInclude(x => x.Recipe)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
