using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using CookbookAPI.Requests.Recipes;
using CookbookAPI.Services;
using CookbookAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace CookbookAPI.Controllers
{
    [Route("api/recipes")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipesService _recipesService;

        public RecipesController(IRecipesService recipesService)
        {
            _recipesService = recipesService;
        }

        [HttpGet]
        [Authorize]
        [Description("Get all recipes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] GetRecipesRequest request)
        {
            var vm = await _recipesService.GetAll(request);
            
            return Ok(vm);
        }

        [HttpGet("{id}")]
        [Authorize]
        [Description("Get specifed recipe")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var vm = await _recipesService.GetById(id);

            return Ok(vm);
        }

        [HttpPost]
        [Authorize]
        [Description("Create new recipe")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create(RecipeRequest request)
        {
            var id = await _recipesService.Create(request);

            return Created($"/api/recipes/{id}", null);
        }

        [HttpPut("{id}")]
        [Authorize]
        [Description("Update recipe")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] int id, RecipeRequest request)
        {
            await _recipesService.Update(id, request);

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        [Description("Delete recipe")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _recipesService.Delete(id);

            return NoContent();
        }

        [HttpGet("favorites")]
        [Authorize]
        [Description("Get all favorites")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllFavorites()
        {
            var vm = await _recipesService.GetAllFavorites();

            return Ok(vm);
        }
    }
}
