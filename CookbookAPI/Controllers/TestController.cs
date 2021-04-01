using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CookbookAPI.ApiClients.Interfaces;
using CookbookAPI.DTOs.MealDB;
using CookbookAPI.Entities;
using CookbookAPI.Mappers;
using CookbookAPI.Mappers.Interfaces;

namespace CookbookAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        public IMapper AutoMapper { get; }
        private readonly IMealApiClient _mealClient;
        private readonly IDtoToEntityMapper<MealRecipeDto, Recipe> _mapper;

        public TestController(IMapper autoMapper, IMealApiClient mealClient, IDtoToEntityMapper<MealRecipeDto, Recipe> mapper)
        {
            AutoMapper = autoMapper;
            _mealClient = mealClient;
            _mapper = mapper;
        }

        public async Task<IActionResult> Get()
        {
            //var stopwatch =  Stopwatch.StartNew();
            //var xd = await _mealClient.GetMealsRecipeByFirstLetterAsync('a');
            //var dtos = await _mealClient.GetAllMealRecipes();
            //stopwatch.Stop();
            //var xddd = stopwatch.Elapsed;

            //var xxx = await _mapper.MapManyAsync(dtos);

            var categoryDtos = await _mealClient.GetAllMealCategories();
            var areaDtos = await _mealClient.GetAllMealAreas();
            var ingredientDtos = await _mealClient.GetAllMealIngredients();

            var categories = AutoMapper.Map<List<Category>>(categoryDtos);
            var areas = AutoMapper.Map<List<Area>>(areaDtos);
            var iingrediens = AutoMapper.Map<List<Ingredient>>(ingredientDtos);

            return Ok();
        }
    }
}
