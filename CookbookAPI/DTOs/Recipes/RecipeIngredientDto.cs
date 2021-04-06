using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookbookAPI.DTOs.Recipes
{
    public class RecipeIngredientDto
    {
        public int IngredientId { get; set; }

        public string Measure { get; set; }
    }
}
