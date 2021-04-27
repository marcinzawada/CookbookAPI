using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookbookAPI.Entities;

namespace CookbookAPI.Repositories.Interfaces
{
    public interface ICategoriesRepository<TCategory> : IBaseRepository<Category>
    {
        public Task<Category> GetByIdWithRecipes(int id);
    }
}
