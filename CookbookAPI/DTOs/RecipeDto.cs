using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookbookAPI.DTOs
{
    public class RecipeDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string User { get; set; }

        public int? UserId { get; set; }

        public string Category { get; set; }

        public int? CategoryId { get; set; }

        public string Area { get; set; }

        public int? AreaId { get; set; }

        public string Thumbnail { get; set; }

        public string Youtube { get; set; }

        public string Source { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string Instructions { get; set; }


        public List<IngredientDto> Ingredients { get; set; } = new List<IngredientDto>();


    }
}
