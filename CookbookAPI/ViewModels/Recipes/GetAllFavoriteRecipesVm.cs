using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookbookAPI.DTOs;

namespace CookbookAPI.ViewModels.Recipes
{
    public class GetAllFavoriteRecipesVm
    {
        public List<BaseRecipeDto> FavoriteRecipes { get; set; } = new List<BaseRecipeDto>();
    }
}
