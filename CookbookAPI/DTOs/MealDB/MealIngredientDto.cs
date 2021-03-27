using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CookbookAPI.DTOs.MealDB
{
    public class MealIngredientDto
    {
        [JsonProperty("strIngredient")]
        public string Name { get; set; }

        [JsonProperty("strDescription")]
        public string Description { get; set; }
    }
}
