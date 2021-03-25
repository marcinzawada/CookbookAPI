using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CookbookAPI.Entities
{
    public class Recipe
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public int? UserId { get; set; }

        public List<RecipeIngredient> RecipeIngredient { get; set; } = new List<RecipeIngredient>();

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public int? CategoryId { get; set; }

        [ForeignKey("AreaId")]
        public virtual Area Area { get; set; }

        public int? AreaId { get; set; }

        public string Instructions { get; set; }

        public string Thumbnail { get; set; }

        public string Youtube { get; set; }

        public string Source { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
