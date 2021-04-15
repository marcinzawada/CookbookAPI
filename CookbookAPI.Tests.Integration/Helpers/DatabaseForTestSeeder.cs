using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookbookAPI.Data;
using CookbookAPI.Entities;
using Microsoft.AspNetCore.Identity;

namespace CookbookAPI.Tests.Integration.Helpers
{
    public class DatabaseForTestSeeder
    {
        private readonly CookbookDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public DatabaseForTestSeeder(CookbookDbContext context, PasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public void Seed()
        {
            if (_context.Recipes.Any()) 
                return;

            var users = SeedUser();
            var ingredients = SeedIngredients();
            var categories = SeedCategories();
            var areas = SeedAreas();
            SeedRecipes(users, ingredients, categories, areas);

        }

        private List<User> SeedUser()
        {
            var users = new List<User>
            {
                new User
                {
                    Email = "test@gmail.com",
                    PasswordHash = _passwordHasher.HashPassword(null, "Pass123!"),
                    Name = "username"
                },
                new User
                {
                    Email = "test2@gmail.com",
                    PasswordHash = _passwordHasher.HashPassword(null, "Pass123!"),
                    Name = "username"
                },
            };

            _context.Users.AddRange(users);
            _context.SaveChanges();

            return users;
        }

        private List<Ingredient> SeedIngredients()
        {
            var ingredients = new List<Ingredient>
            {
                new Ingredient
                {
                    Name = "Tomato"
                },
                new Ingredient
                {
                    Name = "Potato"
                }
            };

            _context.Ingredients.AddRange(ingredients);
            _context.SaveChanges();

            return ingredients;
        }

        private List<Category> SeedCategories()
        {
            var categories = new List<Category>
            {
                new Category
                {
                    Name = "Breakfast"
                },
                new Category
                {
                    Name = "Lunch"
                }
            };

            _context.Categories.AddRange(categories);
            _context.SaveChanges();

            return categories;
        }

        private List<Area> SeedAreas()
        {
            var areas = new List<Area>
            {
                new Area
                {
                    Name = "Poland"
                },
                new Area
                {
                    Name = "France"
                }
            };

            _context.Areas.AddRange(areas);
            _context.SaveChanges();

            return areas;
        }

        private void SeedRecipes(List<User> users, List<Ingredient> ingredients,
            List<Category> categories, List<Area> areas)
        {
            var recipes = new List<Recipe>
            {
                new Recipe
                {
                    Name = "Pizza",
                    UserId = users.Select(x => x.Id).FirstOrDefault(),
                    Area = areas.FirstOrDefault(),
                    Category = categories.FirstOrDefault(),
                    CreatedAt = DateTime.UtcNow,
                    RecipeIngredients = new List<RecipeIngredient>
                    {
                        new RecipeIngredient
                        {
                            Ingredient = ingredients.FirstOrDefault(),
                            Measure = "1 kg"
                        },
                        new RecipeIngredient
                        {
                            Ingredient = ingredients.LastOrDefault(),
                            Measure = "300 ml"
                        }
                    }
                },
                new Recipe
                {
                    Name = "Spaghetti",
                    UserId = users.Select(x => x.Id).LastOrDefault(),
                    Area = areas.LastOrDefault(),
                    Category = categories.LastOrDefault(),
                    CreatedAt = DateTime.UtcNow,
                    RecipeIngredients = new List<RecipeIngredient>
                    {
                        new RecipeIngredient
                        {
                            Ingredient = ingredients.FirstOrDefault(),
                            Measure = "5 kg"
                        },
                        new RecipeIngredient
                        {
                            Ingredient = ingredients.LastOrDefault(),
                            Measure = "100 ml"
                        }
                    }
                }
            };

            _context.Recipes.AddRange(recipes);
            _context.SaveChanges();
        }
    }
}

