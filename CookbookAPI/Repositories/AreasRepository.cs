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
    public class AreasRepository : BaseRepository<Area, CookbookDbContext>, IAreasRepository<Area>
    {
        public AreasRepository(CookbookDbContext context) : base(context)
        {
        }

        public async Task<Area> GetByIdWithRecipes(int id)
        {
            return await _context.Areas
                .Include(x => x.Recipes)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
