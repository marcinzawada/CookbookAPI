using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookbookAPI.DTOs;

namespace CookbookAPI.ViewModels.Areas
{
    public class GetAreaVm
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<BaseRecipeDto> Recipes { get; set; }
    }
}
