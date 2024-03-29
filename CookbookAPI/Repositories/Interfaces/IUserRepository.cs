﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookbookAPI.Entities;

namespace CookbookAPI.Repositories.Interfaces
{
    public interface IUserRepository<TUser> : IBaseRepository<User>
    {
        Task<User> GetByEmail(string email);
    }
}
