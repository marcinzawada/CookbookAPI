using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookbookAPI.Entities;

namespace CookbookAPI.Repositories.Interfaces
{
    public interface IRecipesRepository<TRecipe> : IBaseRepository<Recipe>
    {
        public Task<Recipe> GetRecipeWithDetails(int id);

        public Task<Recipe> GetWithRecipeIngredients(int id);

        public Task<List<Recipe>> GetAllFavoritesByUserId(int userId);
    };

}
