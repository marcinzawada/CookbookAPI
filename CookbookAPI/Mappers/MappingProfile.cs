using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CookbookAPI.DTOs.MealDB;
using CookbookAPI.Entities;

namespace CookbookAPI.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MealCategoryDto, Category>();

            CreateMap<MealAreaDto, Area>();

            CreateMap<MealIngredientDto, Ingredient>();
        }
    }
}
