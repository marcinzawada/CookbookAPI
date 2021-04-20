using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookbookAPI.DTOs;
using CookbookAPI.Requests.Ingredients;
using CookbookAPI.Requests.Recipes;
using CookbookAPI.ViewModels;
using CookbookAPI.ViewModels.Ingredients;
using CookbookAPI.ViewModels.Recipes;

namespace CookbookAPI.Services.Interfaces
{
    public interface IIngredientsService
    {
        public Task<PaginatedList<IngredientDto>> GetAll(GetIngredientsRequest request);

        public Task<GetIngredientVm> GetById(int id);

        public Task<int> Create(IngredientRequest request);

        public Task Update(int id, IngredientRequest request);

        public Task Delete(int id);
    }
}
