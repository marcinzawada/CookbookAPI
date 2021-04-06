using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CookbookAPI.DTOs;
using CookbookAPI.DTOs.Recipes;

namespace CookbookAPI.Requests.Recipes
{
    public class RecipeRequest
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public List<RecipeIngredientDto> Ingredients { get; set; } = new List<RecipeIngredientDto>();

        public int? CategoryId { get; set; }

        public int? AreaId { get; set; }

        [Required]
        [MinLength(20)]
        public string Instructions { get; set; }

        public string Thumbnail { get; set; }

        public string Youtube { get; set; }

        public string Source { get; set; }
    }
}
