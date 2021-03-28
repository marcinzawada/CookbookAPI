using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CookbookAPI.Services.Interfaces
{
    public interface IUserContextService
    {
        public ClaimsPrincipal User { get; }
        public int? GetUserId { get; }
        public string GetUserRole { get; }
    }
}
