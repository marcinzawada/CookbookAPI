using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookbookAPI.Entities;

namespace CookbookAPI.Repositories.Interfaces
{
    public interface IIngredientsRepository<TIngredient> : IBaseRepository<Ingredient>
    {
        public Task<Ingredient> GetByIdWithRecipes(int id);
    }
}
