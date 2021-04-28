using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookbookAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CookbookAPI.Data
{
    public class CookbookDbContext : DbContext
    {
        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<Ingredient> Ingredients { get; set; }
        
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }

        public DbSet<Area> Areas { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserFavoriteRecipe> UserFavoriteRecipes { get; set; }

        public CookbookDbContext(DbContextOptions<CookbookDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RecipeIngredient>()
                .HasKey(ri => new {ri.RecipeId, ri.IngredientId});
            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Recipe)
                .WithMany(r => r.RecipeIngredients)
                .HasForeignKey(ri => ri.RecipeId);
            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Ingredient)
                .WithMany(i => i.RecipeIngredient)
                .HasForeignKey(ri => ri.IngredientId);

            modelBuilder.Entity<UserFavoriteRecipe>()
                .HasKey(x => new { x.RecipeId, x.UserId });
            modelBuilder.Entity<UserFavoriteRecipe>()
                .HasOne(ufr => ufr.User)
                .WithMany(u => u.FavoriteRecipes)
                .HasForeignKey(ufr => ufr.UserId);
            modelBuilder.Entity<UserFavoriteRecipe>()
                .HasOne(ufr => ufr.Recipe)
                .WithMany(r => r.LikedByUsers)
                .HasForeignKey(ufr => ufr.RecipeId);
        }
    }
}
