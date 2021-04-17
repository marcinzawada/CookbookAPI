using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CookbookAPI.Entities
{
    public class Ingredient
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public List<RecipeIngredient> RecipeIngredient { get; set; } = new List<RecipeIngredient>();
    }
}
