using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CookbookAPI.Data;
using CookbookAPI.DTOs;
using CookbookAPI.Entities;
using CookbookAPI.Exceptions;
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

        public async Task<GetAllAreaVm> GetAll()
        {
            var areas = await _areasRepository.GetAll();

            var areaDtos = _mapper.Map<List<AreaDto>>(areas);

            return new GetAllAreaVm
            {
                Areas = areaDtos
            };
        }

        public async Task<GetAreaVm> GetById(int id)
        {
            var area = await _areasRepository.GetByIdWithRecipes(id);

            if (area == null)
                throw new NotFoundException($"Area with id:{id} not found");

            var recipeDtos = _mapper.Map<List<BaseRecipeDto>>(area.Recipes);

            return new GetAreaVm
            {
                Id = area.Id,
                Name = area.Name,
                Recipes = recipeDtos
            };
        }
    }
}
