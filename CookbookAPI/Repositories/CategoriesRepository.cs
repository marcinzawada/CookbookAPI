using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookbookAPI.Data;
using CookbookAPI.Entities;
using CookbookAPI.Repositories.Interfaces;

namespace CookbookAPI.Repositories
{
    public class CategoriesRepository : BaseRepository<Category, CookbookDbContext>, ICategoriesRepository<Category>
    {
        public CategoriesRepository(CookbookDbContext context) : base(context)
        {
        }
    }
}
