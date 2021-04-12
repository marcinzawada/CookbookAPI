using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Bogus;
using CookbookAPI.DTOs.Recipes;
using CookbookAPI.Requests.Recipes;

namespace CookbookAPI.Tests.Integration.DataClasses
{
    public class RecipeDataClass : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var faker = new Faker("en");

            yield return new object[]
            {
                new RecipeRequest
                {
                    Name = "ab", CategoryId = 1, AreaId = 1,
                    Instructions = faker.Lorem.Paragraphs(3),
                    Ingredients = new List<RecipeIngredientDto>
                    {
                        new RecipeIngredientDto {IngredientId = 1, Measure = "100g"}
                    }
                }
            };

            yield return new object[]
            {
                new RecipeRequest
                {
                    Name = "abc", CategoryId = 0, AreaId = 1,
                    Instructions = faker.Lorem.Paragraphs(3),
                    Ingredients = new List<RecipeIngredientDto>
                    {
                        new RecipeIngredientDto {IngredientId = 1, Measure = "100g"}
                    }
                }
            };

            yield return new object[]
            {
                new RecipeRequest
                {
                    Name = "abc",
                    CategoryId = 0,
                    AreaId = 1,
                    Instructions = faker.Lorem.Paragraphs(3),
                    Ingredients = new List<RecipeIngredientDto>
                    {
                        new RecipeIngredientDto {IngredientId = 1, Measure = "100g"}
                    }
                }
            };

            yield return new object[]
            {
                new RecipeRequest
                {
                    Name = "abc",
                    CategoryId = 1,
                    AreaId = 0,
                    Instructions = faker.Lorem.Paragraphs(3),
                    Ingredients = new List<RecipeIngredientDto>
                    {
                        new RecipeIngredientDto {IngredientId = 1, Measure = "100g"}
                    }
                }
            };

            yield return new object[]
            {
                new RecipeRequest
                {
                    Name = "abc",
                    CategoryId = 1,
                    AreaId = 1,
                    Instructions = "toShortInstruction",
                    Ingredients = new List<RecipeIngredientDto>
                    {
                        new RecipeIngredientDto {IngredientId = 1, Measure = "100g"}
                    }
                }
            };

            yield return new object[]
            {
                new RecipeRequest
                {
                    Name = "abc",
                    CategoryId = 1,
                    AreaId = 1,
                    Instructions = faker.Lorem.Paragraphs(3),
                    Ingredients = new List<RecipeIngredientDto>
                    {
                        new RecipeIngredientDto {IngredientId = 0, Measure = "100g"}
                    }
                }
            };

            yield return new object[]
            {
                new RecipeRequest
                {
                    Name = "abc",
                    CategoryId = 1,
                    AreaId = 1,
                    Instructions = faker.Lorem.Paragraphs(3),
                    Ingredients = new List<RecipeIngredientDto>
                    {
                        new RecipeIngredientDto {Measure = "100g"}
                    }
                }
            };

            yield return new object[]
            {
                new RecipeRequest
                {
                    Name = "abc",
                    CategoryId = 1,
                    AreaId = 1,
                    Instructions = faker.Lorem.Paragraphs(3),
                    Ingredients = new List<RecipeIngredientDto>
                    {
                        new RecipeIngredientDto {IngredientId = 1}
                    }
                }
            };

            yield return new object[]
            {
                new RecipeRequest
                {
                    Name = "abc",
                    CategoryId = 1,
                    AreaId = 1,
                    Instructions = faker.Lorem.Paragraphs(3),
                    Ingredients = new List<RecipeIngredientDto>
                    {
                        new RecipeIngredientDto {IngredientId = 1, Measure = ""}
                    }
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
}
