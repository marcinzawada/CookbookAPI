using System.Collections.Generic;
using System.Threading.Tasks;
using CookbookAPI.DTOs.MealDB;

namespace CookbookAPI.ApiClients.Interfaces
{
    public interface IMealApiClient
    {
        public Task<List<MealRecipeDto>> GetMealsRecipeByFirstLetterAsync(char firstLetter);

        public Task<List<MealRecipeDto>> GetAllMealRecipes();

        public Task<List<MealCategoryDto>> GetAllMealCategories();

        public Task<List<MealAreaDto>> GetAllMealAreas();

        public Task<List<MealIngredientDto>> GetAllMealIngredients();
    }
}
