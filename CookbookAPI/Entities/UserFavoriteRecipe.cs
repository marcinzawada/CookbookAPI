using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CookbookAPI.Entities
{
    public class UserFavoriteRecipe
    {
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public int RecipeId { get; set; }

        [ForeignKey(nameof(RecipeId))]
        public Recipe Recipe { get; set; }
    }
}
