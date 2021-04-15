using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookbookAPI.Entities
{
    public class RecipeIngredient : IEquatable<RecipeIngredient>
    {
        public virtual Recipe Recipe { get; set; }

        public int RecipeId { get; set; }

        public virtual Ingredient Ingredient { get; set; }

        public int IngredientId { get; set; }

        public string Measure { get; set; }

        public bool Equals(RecipeIngredient other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return RecipeId == other.RecipeId && IngredientId == other.IngredientId && Measure == other.Measure;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RecipeIngredient) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(RecipeId, IngredientId, Measure);
        }
    }
}
