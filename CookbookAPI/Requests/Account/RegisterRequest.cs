using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookbookAPI.Requests.Account
{
    public class RegisterRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string Name { get; set; }
    }
}
