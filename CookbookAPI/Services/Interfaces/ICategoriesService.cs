using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookbookAPI.ViewModels.Categories;

namespace CookbookAPI.Services.Interfaces
{
    public interface ICategoriesService
    {
        public Task<GetAllCategoriesVm> GetAll();

        public Task<GetCategoryVm> GetById(int id);
    }
}
