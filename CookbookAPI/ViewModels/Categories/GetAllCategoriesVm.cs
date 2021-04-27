using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookbookAPI.DTOs;

namespace CookbookAPI.ViewModels.Categories
{
    public class GetAllCategoriesVm
    {
        public List<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
    }
}
