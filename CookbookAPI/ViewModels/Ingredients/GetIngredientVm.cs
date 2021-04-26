using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookbookAPI.DTOs;
using CookbookAPI.DTOs.Ingredients;

namespace CookbookAPI.ViewModels.Ingredients
{
    public class GetIngredientVm
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public List<IngredientRecipeDto> Recipes { get; set; } = new List<IngredientRecipeDto>();
    }
}
