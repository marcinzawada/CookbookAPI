using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookbookAPI.Data;
using CookbookAPI.Entities;
using CookbookAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CookbookAPI.Repositories
{
    public class UserRepository : BaseRepository<User, CookbookDbContext>, IUserRepository<User>
    {
        public UserRepository(CookbookDbContext context) : base(context)
        {

        }

        public Task<User> GetByEmail(string email)
        {
            return _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
        }
    }
}
