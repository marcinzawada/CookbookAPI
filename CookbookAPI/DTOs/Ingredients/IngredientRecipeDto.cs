using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookbookAPI.DTOs.Ingredients
{
    public class IngredientRecipeDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Thumbnail { get; set; }
    }
}
