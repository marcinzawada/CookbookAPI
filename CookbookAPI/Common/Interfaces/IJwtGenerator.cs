using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookbookAPI.Entities;

namespace CookbookAPI.Common.Interfaces
{
    public interface IJwtGenerator
    {
        public string Generate(User user);
    }
}
