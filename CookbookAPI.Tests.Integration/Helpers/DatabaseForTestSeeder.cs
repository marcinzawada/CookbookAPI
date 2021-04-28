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
            SeedFavoriteRecipes();

        }

        private List<User> SeedUser()
        {
            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Email = "test@gmail.com",
                    PasswordHash = _passwordHasher.HashPassword(null, "Pass123!"),
                    Name = "username"
                },
                new User
                {
                    Id = 2,
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
                    Id = 1,
                    Name = "Tomato",
                    UserId = 1
                },
                new Ingredient
                {
                    Id = 2,
                    Name = "Potato",
                    UserId = 2
                },
                new Ingredient
                {
                    Id = 3,
                    Name = "Onion",
                    UserId = 1
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
                    Id = 1,
                    Name = "Breakfast"
                },
                new Category
                {
                    Id = 2,
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
                    Id = 1,
                    Name = "Poland"
                },
                new Area
                {
                    Id = 2,
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
                    Id = 1,
                    Name = "Pizza",
                    UserId = 1,
                    Area = areas.FirstOrDefault(),
                    Category = categories.FirstOrDefault(),
                    CreatedAt = DateTime.UtcNow,
                    RecipeIngredients = new List<RecipeIngredient>
                    {
                        new RecipeIngredient
                        {
                            IngredientId = 1,
                            Measure = "1 kg"
                        },
                        new RecipeIngredient
                        {
                            IngredientId = 2,
                            Measure = "300 ml"
                        }
                    }
                },
                new Recipe
                {
                    Id = 2,
                    Name = "Spaghetti",
                    UserId = 2,
                    Area = areas.LastOrDefault(),
                    Category = categories.LastOrDefault(),
                    CreatedAt = DateTime.UtcNow,
                    RecipeIngredients = new List<RecipeIngredient>
                    {
                        new RecipeIngredient
                        {
                            IngredientId = 1,
                            Measure = "5 kg"
                        },
                        new RecipeIngredient
                        {
                            IngredientId = 2,
                            Measure = "100 ml"
                        }
                    }
                },
                new Recipe
                {
                    Id = 3,
                    Name = "Kebab",
                    UserId = 1,
                    Area = areas.LastOrDefault(),
                    Category = categories.LastOrDefault(),
                    CreatedAt = DateTime.UtcNow,
                    RecipeIngredients = new List<RecipeIngredient>
                    {
                        new RecipeIngredient
                        {
                            IngredientId = 1,
                            Measure = "5 kg"
                        },
                        new RecipeIngredient
                        {
                            IngredientId = 3,
                            Measure = "100 ml"
                        },
                    }
                }
            };

            _context.Recipes.AddRange(recipes);
            _context.SaveChanges();
        }

        private void SeedFavoriteRecipes()
        {
            var favorites = new List<UserFavoriteRecipe>
            {
                new UserFavoriteRecipe
                {
                    UserId = 1,
                    RecipeId = 1
                },
                new UserFavoriteRecipe
                {
                    UserId = 1,
                    RecipeId = 2
                },
            };

            _context.UserFavoriteRecipes.AddRange(favorites);
            _context.SaveChanges();
        }
    }
}

