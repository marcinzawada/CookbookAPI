using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CookbookAPI.Data;
using CookbookAPI.DTOs;
using CookbookAPI.Entities;
using CookbookAPI.Repositories.Interfaces;
using CookbookAPI.Services.Interfaces;
using CookbookAPI.ViewModels.Areas;
using Microsoft.AspNetCore.Authorization;

namespace CookbookAPI.Services
{
    public class AreasService : BaseService, IAreasService
    {
        private readonly IAreasRepository<Area> _areasRepository;

        public AreasService(CookbookDbContext context, IMapper mapper,
            IUserContextService userContextService, IAuthorizationService authorizationService, 
            IAreasRepository<Area> areasRepository)
            : base(context, mapper, userContextService, authorizationService)
        {
            _areasRepository = areasRepository;
        }

        public async Task<GetAreaVm> GetAll()
        {
            var areas = await _areasRepository.GetAll();

            var areaDtos = _mapper.Map<List<AreaDto>>(areas);

            return new GetAreaVm
            {
                Areas = areaDtos
            };
        }
    }
}
