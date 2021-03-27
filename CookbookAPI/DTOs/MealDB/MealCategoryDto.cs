using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CookbookAPI.DTOs.MealDB
{
    public class MealCategoryDto
    {
        [JsonProperty("strCategory")]
        public string Name { get; set; }
    }
}
