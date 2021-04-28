using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookbookAPI.Data;
using CookbookAPI.Entities;
using CookbookAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CookbookAPI.Repositories
{
    public class RecipesRepository : BaseRepository<Recipe, CookbookDbContext>, IRecipesRepository<Recipe>
    {
        public RecipesRepository(CookbookDbContext context) : base(context)
        {
        }

        public async Task<Recipe> GetRecipeWithDetails(int id)
        {
            var recipe = await _context.Recipes
                .Include(x => x.RecipeIngredients)
                .ThenInclude(x => x.Ingredient)
                .Include(x => x.Area)
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id);

            return recipe;
        }

        public async Task<Recipe> GetWithRecipeIngredients(int id)
        {
            var recipe = await _context.Recipes
                .Include(x => x.RecipeIngredients)
                .FirstOrDefaultAsync(x => x.Id == id);

            return recipe;
        }

        public async Task<List<Recipe>> GetAllFavoritesByUserId(int userId)
        {
            return await _context.Recipes
                .Include(x => x.LikedByUsers)
                .Where(x => x.LikedByUsers
                    .Any(y =>
                        y.UserId == userId))
                .ToListAsync();
        }

        public async Task AddRecipeToFavorite(int recipeId, int userId)
        {
            await _context.UserFavoriteRecipes.AddAsync(new UserFavoriteRecipe
            {
                UserId = userId,
                RecipeId = recipeId
            });

            await _context.SaveChangesAsync();
        }

        public async Task<UserFavoriteRecipe> GetFavorite(int recipeId, int userId)
        {
            return await _context.UserFavoriteRecipes
                 .FirstOrDefaultAsync(x =>
                     x.UserId == userId &&
                     x.RecipeId == recipeId);
        }

        public async Task DeleteRecipeFromFavorite(UserFavoriteRecipe favorite)
        {
            _context.UserFavoriteRecipes.Remove(favorite);
            await _context.SaveChangesAsync();
        }
    }
}
