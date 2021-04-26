using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CookbookAPI.Data;
using CookbookAPI.DTOs;
using CookbookAPI.DTOs.Ingredients;
using CookbookAPI.Entities;
using CookbookAPI.Exceptions;
using CookbookAPI.Extensions;
using CookbookAPI.Repositories.Interfaces;
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
        private readonly IIngredientsRepository<Ingredient> _ingredientsRepository;

        public IngredientsService(CookbookDbContext context, IMapper mapper,
            IUserContextService userContextService, IAuthorizationService authorizationService, 
            IIngredientsRepository<Ingredient> ingredientsRepository)
            : base(context, mapper, userContextService, authorizationService)
        {
            _ingredientsRepository = ingredientsRepository;
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

        public async Task<GetIngredientVm> GetById(int id)
        {
            var ingredient = await _ingredientsRepository.GetByIdWithRecipes(id);
            if (ingredient == null)
                throw new NotFoundException($"Ingredient with id: {id} not found");

            var recipes = ingredient.RecipeIngredient.Select(x => x.Recipe).ToList();

            var recipesDto =
                _mapper.Map<List<IngredientRecipeDto>>(recipes);

            return new GetIngredientVm
            {
                Name = ingredient.Name,
                Description = ingredient.Description,
                Recipes = recipesDto
            };
        }

        public async Task<int> Create(IngredientRequest request)
        {
            var userId = _userContextService.GetUserId;
            if (userId == null)
                throw new ForbidException();

            var newIngredient = _mapper.Map<Ingredient>(request);

            newIngredient.UserId = userId;

            await _ingredientsRepository.Add(newIngredient);

            return newIngredient.Id;
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
