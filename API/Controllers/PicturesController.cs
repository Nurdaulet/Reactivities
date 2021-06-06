

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.SwaggerExamples;
using Application.Core;
using Application.Pictures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    public class PicturesController : BaseApiController
    {
        /// <summary>
        /// Get details for given picture
        /// </summary>
        /// <returns>Returns the corresponding picture</returns>
        [HttpGet("{id}")]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            SwaggerDocumentation.PictureConstants.SuccessfulGetPictureDetailsRequestDescriptionMessage,
            typeof(Result<PictureDetailsResponseModel>))]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            SwaggerDocumentation.PictureConstants.BadRequestDescriptionMessage)]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await Mediator.Send(new Details.Query{Id = id});
            return HandleResult(result);
        }

        /// <summary>
        /// Uploads pictures
        /// </summary>
        /// <returns>Collection of the corresponding pictures with their Id's and urls</returns>
        [HttpPost]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            SwaggerDocumentation.PictureConstants.SuccessfulGetRequestDescriptionMessage,
            typeof(Result<List<PictureResponseModel>>))]
        public async Task<IActionResult> Post([FromBody] CreatePictureModel model)
        {
            var result = await Mediator.Send(new Create.Command {Picture = model});
            return HandleResult(result);
        }

        /// <summary>
        /// Deletes picture
        /// </summary>
        [HttpDelete("{id}")]
        [SwaggerResponse(
            StatusCodes.Status204NoContent,
            SwaggerDocumentation.PictureConstants.SuccessfulDeleteRequestDescriptionMessage)]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            SwaggerDocumentation.PictureConstants.BadRequestDescriptionMessage)]
        public async Task<IActionResult> Delete(Guid id, [FromBody] DeletePictureModel model)
        {
            model.PictureId = id;

            var result = await Mediator.Send(new Delete.Command{Picture = model});
            return HandleResult(result);
        }
    }
}
