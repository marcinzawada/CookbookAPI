using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookbookAPI.DTOs;

namespace CookbookAPI.ViewModels.Areas
{
    public class GetAllAreaVm
    {
        public List<AreaDto> Areas { get; set; }
    }
}
