using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using CookbookAPI.Requests.Ingredients;
using CookbookAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CookbookAPI.Controllers
{
    [Route("api/ingredients")]
    [ApiController]
    public class IngredientsController : ControllerBase
    {
        private readonly IIngredientsService _ingredientsService;

        public IngredientsController(IIngredientsService ingredientsService)
        {
            _ingredientsService = ingredientsService;
        }

        [HttpGet]
        [Authorize]
        [Description("Get all ingredients")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] GetIngredientsRequest request)
        {
            var vm = await _ingredientsService.GetAll(request);

            return Ok(vm);
        }

        [HttpGet("{id}/recipes")]
        [Authorize]
        [Description("Get specifed ingredient with all recipes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var vm = await _ingredientsService.GetById(id);

            return Ok(vm);
        }

        [HttpPost]
        [Authorize]
        [Description("Create new ingredient")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] IngredientRequest request)
        { 
            var id = await _ingredientsService.Create(request);

            return Created($"api/ingredients/{id}", null);
        }

        [HttpPut("{id}")]
        [Authorize]
        [Description("Update ingredient")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] IngredientRequest request, 
            [FromRoute] int id)
        {
            await _ingredientsService.Update(id, request);

            return Ok();
        }
    }
}
