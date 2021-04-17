using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CookbookAPI.Data;
using CookbookAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace CookbookAPI.Services
{
    public class BaseService
    {
        protected readonly CookbookDbContext _context;
        protected readonly IMapper _mapper;
        protected readonly IUserContextService _userContextService;
        protected readonly IAuthorizationService _authorizationService;

        public BaseService(CookbookDbContext context, IMapper mapper, IUserContextService userContextService, IAuthorizationService authorizationService)
        {
            _context = context;
            _mapper = mapper;
            _userContextService = userContextService;
            _authorizationService = authorizationService;
        }
    }
}
