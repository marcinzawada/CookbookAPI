﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CookbookAPI.Requests.Ingredients
{
    public class IngredientRequest
    {
        [Required]
        [MinLength(2)]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
