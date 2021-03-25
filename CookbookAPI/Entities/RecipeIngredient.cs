using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookbookAPI.Entities
{
    public class RecipeIngredient
    {
        public virtual Recipe Recipe { get; set; }

        public int RecipeId { get; set; }

        public virtual Ingredient Ingredient { get; set; }

        public int IngredientId { get; set; }

        public string Measure { get; set; }
    }
}
