
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Common;
using API.SwaggerExamples;
using Application.Categories;
using Application.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    public class CategoriesController : BaseApiController
    {
        private const int CachedTimeInMinutes = 3600;

        [HttpGet]
        [Cached(CachedTimeInMinutes)]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            SwaggerDocumentation.CategoriesConstants.SuccessfulGetRequestMessage,
            typeof(Result<List<CategoryDto>>))]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            SwaggerDocumentation.CategoriesConstants.BadRequestDescriptionMessage,
            typeof(string))]
        public async Task<IActionResult> Get()
        {
            var result = await Mediator.Send(new List.Query());
            return HandleResult(result);
        }
    }
}
