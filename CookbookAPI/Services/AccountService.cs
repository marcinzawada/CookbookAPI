using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookbookAPI.Common.Interfaces;
using CookbookAPI.Entities;
using CookbookAPI.Exceptions;
using CookbookAPI.Repositories;
using CookbookAPI.Repositories.Interfaces;
using CookbookAPI.Requests.Account;
using CookbookAPI.Services.Interfaces;
using CookbookAPI.ViewModels.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace CookbookAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly IJwtGenerator _jwtGenerator;

        public AccountService(UserRepository userRepository, UserManager<User> userManager, IJwtGenerator jwtGenerator)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _jwtGenerator = jwtGenerator;
        }

        public async Task RegisterUser(RegisterRequest request)
        {
            var newUser = new User()
            {
                Email = request.Email,
            };

            var hashedPassword = _userManager.PasswordHasher.HashPassword(newUser, request.Password);
            newUser.PasswordHash = hashedPassword;

            await _userRepository.Add(newUser);
        }

        public async Task<LoginVm> Login(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Email);

            if (user is null)
                throw new BadRequestException("Invalid email or password");

            var passwordIsCorrect = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash ,request.Password);

            if (passwordIsCorrect == PasswordVerificationResult.Failed)
                throw new BadRequestException("Invalid email or password");

            var jwt = _jwtGenerator.Generate(user);

            var loginVm = new LoginVm
            {
                Id = user.Id,
                Token = jwt
            };

            return loginVm;
        }
    }
}
