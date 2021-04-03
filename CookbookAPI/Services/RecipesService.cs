using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CookbookAPI.Data;
using CookbookAPI.DTOs;
using CookbookAPI.Exceptions;
using CookbookAPI.Extensions;
using CookbookAPI.Repositories;
using CookbookAPI.Requests.Recipes;
using CookbookAPI.ViewModels;
using CookbookAPI.ViewModels.Recipes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CookbookAPI.Services
{
    public class RecipesService
    {
        private readonly RecipesRepository _recipesRepository;
        private readonly CookbookDbContext _context;
        private readonly IMapper _mapper;

        public RecipesService(RecipesRepository recipesRepository, CookbookDbContext context, IMapper mapper)
        {
            _recipesRepository = recipesRepository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<RecipeDto>> GetAll(GetRecipesRequest request)
        {
            var paginatedRecipes = await _context.Recipes
                .Include(x => x.RecipeIngredients)
                .ThenInclude(x => x.Ingredient)
                .Include(x => x.Area)
                .Include(x => x.Category)
                .ProjectTo<RecipeDto>(_mapper.ConfigurationProvider)
                .Where(x => 
                    string.IsNullOrEmpty(request.SearchPhrase) 
                    || x.Instructions.ToLower().Contains(request.SearchPhrase.ToLower())
                    || x.Name.ToLower().Contains(request.SearchPhrase.ToLower())
                    || x.Category.ToLower().Contains(request.SearchPhrase.ToLower()))
                .PaginatedListAsync(request.PageNumber, request.PageSize, request.SortBy, request.SortDirection);

            return paginatedRecipes;
        }

        public async Task<GetRecipeVm> GetById(int id)
        {
            var recipe = await _recipesRepository.GetRecipeWithDetails(id);

            if (recipe == null)
                throw new NotFoundException($"Recipe with id: {id} not found");

            var recipeDto = _mapper.Map<RecipeDto>(recipe);

            return new GetRecipeVm{Recipe = recipeDto};
        }
    }
}
