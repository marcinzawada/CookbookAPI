using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CookbookAPI.DTOs.Recipes
{
    public class RecipeIngredientDto
    {
        [Required]
        public int IngredientId { get; set; }

        [Required]
        public string Measure { get; set; }
    }
}
