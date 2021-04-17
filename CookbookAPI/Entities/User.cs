using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CookbookAPI.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string PasswordHash { get; set; }

        public List<Recipe> Recipes { get; set; } = new List<Recipe>();

        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>(); 
    }
}
