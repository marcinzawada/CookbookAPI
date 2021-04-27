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
    public class CategoriesRepository : BaseRepository<Category, CookbookDbContext>, ICategoriesRepository<Category>
    {
        public CategoriesRepository(CookbookDbContext context) : base(context)
        {
        }

        public async Task<Category> GetByIdWithRecipes(int id)
        {
            return await _context.Categories
                .Include(x => x.Recipes)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
