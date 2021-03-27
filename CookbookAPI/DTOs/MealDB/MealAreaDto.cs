using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CookbookAPI.DTOs.MealDB
{
    public class MealAreaDto
    {
        [JsonProperty("strArea")]
        public string Name { get; set; }
    }
}
