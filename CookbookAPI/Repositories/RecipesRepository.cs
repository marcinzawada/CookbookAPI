using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookbookAPI.Data;
using CookbookAPI.Entities;

namespace CookbookAPI.Repositories
{
    public class RecipesRepository : BaseRepository<Recipe, CookbookDbContext>
    {
        public RecipesRepository(CookbookDbContext context) : base(context)
        {
        }
    }
}
