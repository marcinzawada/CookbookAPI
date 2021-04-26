using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using CookbookAPI.Requests.Recipes;
using CookbookAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CookbookAPI.Controllers
{
    [Route("api/areas")]
    [ApiController]
    public class AreasController : ControllerBase
    {
        private readonly IAreasService _areasService;

        public AreasController(IAreasService areasService)
        {
            _areasService = areasService;
        }

        [HttpGet]
        [Authorize]
        [Description("Get all areas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var vm = await _areasService.GetAll();

            return Ok(vm);
        }

    }
}
