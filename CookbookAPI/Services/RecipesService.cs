using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CookbookAPI.Authorization;
using CookbookAPI.Data;
using CookbookAPI.DTOs;
using CookbookAPI.Entities;
using CookbookAPI.Exceptions;
using CookbookAPI.Extensions;
using CookbookAPI.Repositories;
using CookbookAPI.Repositories.Interfaces;
using CookbookAPI.Requests.Recipes;
using CookbookAPI.Services.Interfaces;
using CookbookAPI.ViewModels;
using CookbookAPI.ViewModels.Recipes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CookbookAPI.Services
{
    public class RecipesService : BaseService, IRecipesService
    {
        private readonly IRecipesRepository<Recipe> _recipesRepository;

        public RecipesService(IRecipesRepository<Recipe> recipesRepository, CookbookDbContext context,
            IMapper mapper, IUserContextService userContextService, IAuthorizationService authorizationService) 
            : base(context, mapper, userContextService, authorizationService)
        {
            _recipesRepository = recipesRepository;
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

        public async Task<int> Create(RecipeRequest request)
        {
            var userId = _userContextService.GetUserId;
            if (userId is null)
                throw new ForbidException();

            var newRecipe = _mapper.Map<Recipe>(request);
            newRecipe.UserId = userId;

            await _recipesRepository.Add(newRecipe);

            return newRecipe.Id;
        }

        public async Task Update(int id, RecipeRequest request)
        {
            var recipe = await _recipesRepository.GetWithRecipeIngredients(id);

            if (recipe is null)
                throw new NotFoundException("Recipe not found");

            var authorizationResult = await _authorizationService.AuthorizeAsync(_userContextService.User, recipe,
                new RecipeOperationRequirement(ResourceOperation.Update));

            if (!authorizationResult.Succeeded)
                throw new ForbidException();

            var newRecipeIngredients = request.Ingredients.Select(x =>
                new RecipeIngredient {IngredientId = x.IngredientId, Measure = x.Measure, RecipeId = id}).ToList();

            foreach (var recipeIngredient in recipe.RecipeIngredients.ToList())
            {
                if (!newRecipeIngredients.Contains(recipeIngredient))
                    recipe.RecipeIngredients.Remove(recipeIngredient);
            }

            foreach (var newRecipeIngredient in newRecipeIngredients)
            {
                if (!recipe.RecipeIngredients.Any(x => x.Equals(newRecipeIngredient)))
                    recipe.RecipeIngredients.Add(newRecipeIngredient);
            }

            recipe.AreaId = request.AreaId;
            recipe.CategoryId = request.CategoryId;
            recipe.Instructions = request.Instructions;
            recipe.Name = request.Name;
            recipe.Youtube = recipe.Youtube;
            recipe.Source = recipe.Source;
            recipe.UpdatedAt = DateTime.UtcNow;
            
            await _recipesRepository.Update(recipe);
        }

        public async Task Delete(int id)
        {
            var recipe = await _recipesRepository.Get(id);

            if (recipe is null)
                throw new  NotFoundException("Recipe not found");

            var authorizationResult = await _authorizationService.AuthorizeAsync(_userContextService.User, recipe,
                new RecipeOperationRequirement(ResourceOperation.Delete));

            if (!authorizationResult.Succeeded)
                throw new ForbidException();

            await _recipesRepository.Delete(id);
        }

        public async Task<GetAllFavoriteRecipesVm> GetAllFavorites()
        {
            var userId = _userContextService.GetUserId;
            if (userId == null)
                throw new ForbidException();

            var recipes = await _recipesRepository.GetAllFavoritesByUserId((int)userId);

            var recipeDtos = _mapper.Map<List<BaseRecipeDto>>(recipes);

            return new GetAllFavoriteRecipesVm
            {
                FavoriteRecipes = recipeDtos
            };
        }
    }
}
