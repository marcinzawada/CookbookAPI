using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CookbookAPI.DTOs;
using CookbookAPI.DTOs.MealDB;
using CookbookAPI.Entities;
using CookbookAPI.Requests.Recipes;

namespace CookbookAPI.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MealCategoryDto, Category>();

            CreateMap<MealAreaDto, Area>();

            CreateMap<MealIngredientDto, Ingredient>();

            CreateMap<Ingredient, IngredientDto>();

            CreateMap<Recipe, RecipeDto>()
                .ForMember(x => x.User, opt => opt.MapFrom(x => x.User.Name))
                .ForMember(x => x.Category, opt => opt.MapFrom(x => x.Category.Name))
                .ForMember(x => x.Area, opt => opt.MapFrom(x => x.Area.Name))
                .ForMember(x => x.Ingredients, opt => opt.MapFrom(x => x.RecipeIngredients.Select(y => new IngredientDto { Id = y.IngredientId, Description = y.Ingredient.Description, Name = y.Ingredient.Name, Measure = y.Measure })));

            CreateMap<RecipeRequest, Recipe>()
                .ForMember(x => x.RecipeIngredients, opt =>
                        opt.MapFrom(x => x.Ingredients.Select(y => new RecipeIngredient
                        { IngredientId = y.IngredientId, Measure = y.Measure })))
                .ForMember(x => x.CreatedAt, opt =>
                    opt.MapFrom(y => DateTime.UtcNow));
        }
    }
}
