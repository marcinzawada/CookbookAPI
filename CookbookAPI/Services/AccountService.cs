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
        private readonly IUserRepository<User> _userRepository;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountService(IUserRepository<User> userRepository, IJwtGenerator jwtGenerator, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _jwtGenerator = jwtGenerator;
            _passwordHasher = passwordHasher;
        }

        public async Task RegisterUser(RegisterRequest request)
        {
            var newUser = new User()
            {
                Email = request.Email,
                Name = request.Name
            };

            var hashedPassword = _passwordHasher.HashPassword(newUser, request.Password);
            newUser.PasswordHash = hashedPassword;

            await _userRepository.Add(newUser);
        }

        public async Task<LoginVm> Login(LoginRequest request)
        {
            var user = await _userRepository.GetByEmail(request.Email);

            if (user is null)
                throw new BadRequestException("Invalid email or password");

            var passwordIsCorrect = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash ,request.Password);

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
