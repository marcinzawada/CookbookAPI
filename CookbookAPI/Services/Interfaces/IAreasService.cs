using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookbookAPI.ViewModels.Areas;

namespace CookbookAPI.Services.Interfaces
{
    public interface IAreasService
    {
        public Task<GetAreaVm> GetAll();
    }
}
