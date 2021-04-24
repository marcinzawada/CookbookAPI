using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CookbookAPI.Data;
using CookbookAPI.Entities;
using CookbookAPI.Repositories.Interfaces;

namespace CookbookAPI.Repositories
{
    public class IngredientsRepository : BaseRepository<Ingredient, CookbookDbContext>, IIngredientsRepository<Ingredient>
    {
        public IngredientsRepository(CookbookDbContext context) : base(context)
        {
        }
    }
}
