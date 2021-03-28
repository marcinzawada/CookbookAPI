using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookbookAPI.Requests.Account;
using CookbookAPI.ViewModels.Account;

namespace CookbookAPI.Services.Interfaces
{
    public interface IAccountService
    {
        public Task RegisterUser(RegisterRequest request);

        public Task<LoginVm> Login(LoginRequest request);
    }
}
