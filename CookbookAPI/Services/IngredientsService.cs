using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CookbookAPI.Data;
using CookbookAPI.DTOs;
using CookbookAPI.Extensions;
using CookbookAPI.Requests.Ingredients;
using CookbookAPI.Requests.Recipes;
using CookbookAPI.Services.Interfaces;
using CookbookAPI.ViewModels;
using CookbookAPI.ViewModels.Ingredients;
using CookbookAPI.ViewModels.Recipes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CookbookAPI.Services
{
    public class IngredientsService : BaseService, IIngredientsService
    {
        public IngredientsService(CookbookDbContext context, IMapper mapper,
            IUserContextService userContextService, IAuthorizationService authorizationService) 
            : base(context, mapper, userContextService, authorizationService)
        {
        }

        public async Task<PaginatedList<IngredientDto>> GetAll(GetIngredientsRequest request)
        {
            var paginatedRecipes = await _context.Ingredients
                .ProjectTo<IngredientDto>(_mapper.ConfigurationProvider)
                .Where(x =>
                    string.IsNullOrEmpty(request.SearchPhrase)
                    || x.Name.ContainsIgnoreCase(request.SearchPhrase)
                    || x.Description.ContainsIgnoreCase(request.SearchPhrase))
                .PaginatedListAsync(request.PageNumber, request.PageSize, request.SortBy, request.SortDirection);

            return paginatedRecipes;
        }

        public Task<GetIngredientVm> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> Create(IngredientRequest request)
        {
            throw new NotImplementedException();
        }

        public Task Update(int id, IngredientRequest request)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
