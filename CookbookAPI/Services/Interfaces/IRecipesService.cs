using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookbookAPI.DTOs;
using CookbookAPI.Requests.Recipes;
using CookbookAPI.ViewModels;
using CookbookAPI.ViewModels.Recipes;

namespace CookbookAPI.Services.Interfaces
{
    public interface IRecipesService
    {
        public Task<PaginatedList<RecipeDto>> GetAll(GetRecipesRequest request);

        public Task<GetRecipeVm> GetById(int id);

        public Task<int> Create(RecipeRequest request);

        public Task Update(int id, RecipeRequest request);

        public Task Delete(int id);

        public Task<GetAllFavoriteRecipesVm> GetAllFavorites();

        public Task AddToFavorites(int id);
        
        public Task DeleteFromFavorites(int id);

    }
}
