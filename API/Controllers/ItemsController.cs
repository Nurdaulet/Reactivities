using System;
using System.Threading.Tasks;
using API.Common;
using API.SwaggerExamples;
using Application.Core;
using Application.Items;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    public class ItemsController: BaseApiController
    {
        private const int CachingTimeInMinutes = 10;

        /// <summary>
        /// Retrieves all items (max 24 per request)
        /// </summary>
        [HttpGet]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            SwaggerDocumentation.ItemConstants.SuccessfulGetRequestMessage,
            typeof(Result<PagedList<ItemDto>>))]
        [Cached(CachingTimeInMinutes)]
        public async Task<IActionResult> GetItems([FromQuery] ItemParams param)
        {
            return HandlePagedResult(await Mediator.Send(new List.Query { Params = param }));
        }

        /// <summary>
        /// Retrieves item with given id
        /// </summary>
        [HttpGet("{id}")]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            SwaggerDocumentation.ItemConstants.SuccessfulGetRequestWithIdDescriptionMessage,
            typeof(Result<ItemDto>))]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            SwaggerDocumentation.ItemConstants.BadRequestDescriptionMessage,
            typeof(string))]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await Mediator.Send(new Details.Query{Id = id});
            return HandleResult(result);
        }

        /// <summary>
        /// Creates item
        /// </summary>
        [HttpPost]
        [SwaggerResponse(
            StatusCodes.Status201Created,
            SwaggerDocumentation.ItemConstants.SuccessfulPostRequestDescriptionMessage)]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            SwaggerDocumentation.ItemConstants.BadRequestDescriptionMessage,
            typeof(string))]
        [SwaggerResponse(
            StatusCodes.Status401Unauthorized,
            SwaggerDocumentation.UnauthorizedDescriptionMessage)]
        public async Task<IActionResult> Post([FromForm] CreateItemModel model)
        {
            return HandleResult(await Mediator.Send(new Create.Command {Item = model}));
        }

        /// <summary>
        /// Updates item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        [HttpPut("{id}")]
        [SwaggerResponse(
            StatusCodes.Status204NoContent,
            SwaggerDocumentation.ItemConstants.SuccessfulPutRequestDescriptionMessage)]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            SwaggerDocumentation.ItemConstants.BadRequestDescriptionMessage,
            typeof(string))]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            SwaggerDocumentation.ItemConstants.NotFoundDescriptionMessage)]
        [SwaggerResponse(
            StatusCodes.Status401Unauthorized,
            SwaggerDocumentation.UnauthorizedDescriptionMessage)]
        public async Task<IActionResult> Put(Guid id, [FromForm] UpdateItemModel model)
        {
            model.Id = id;
            return HandleResult(await Mediator.Send(new Edit.Command { UpdateItem = model }));
        }

        /// <summary>
        /// Deletes item
        /// </summary>
        /// <param name="id"></param>
        [Authorize]
        [HttpDelete("{id}")]
        [SwaggerResponse(
            StatusCodes.Status204NoContent,
            SwaggerDocumentation.ItemConstants.SuccessfulDeleteRequestDescriptionMessage)]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            SwaggerDocumentation.ItemConstants.BadRequestDescriptionMessage)]
        [SwaggerResponse(
            StatusCodes.Status401Unauthorized,
            SwaggerDocumentation.UnauthorizedDescriptionMessage)]
        public async Task<IActionResult> Delete(Guid id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command{Id = id}));
        }
    }
}