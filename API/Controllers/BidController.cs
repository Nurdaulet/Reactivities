

using System;
using System.Threading.Tasks;
using API.SwaggerExamples;
using Application.Bid;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    public class BidController : BaseApiController
    {
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK,
            SwaggerDocumentation.BidConstants.SuccessfulPostRequestDescriptionMessage)]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            SwaggerDocumentation.BidConstants.BadRequestOnPostRequestDescriptionMessage,
            typeof(string))]
        [SwaggerResponse(
            StatusCodes.Status401Unauthorized,
            SwaggerDocumentation.UnauthorizedDescriptionMessage)]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            SwaggerDocumentation.BidConstants.NotFoundOnPostRequestDescriptionMessage)]
        public async Task<IActionResult> Post([FromBody] Create.Command model)
        {
            var result = await Mediator.Send(model);
            return HandleResult(result);
        }

        [HttpGet]
        [Route("getHighestBid/{itemId?}")]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            SwaggerDocumentation.BidConstants.GetHighestBidDescriptionMessage,
            typeof(GetHighestBidDetailsResponseModel))]
        public async Task<IActionResult> GetHighestBid(Guid itemId)
        {
            var result = await Mediator.Send(new GetHighestBidDetails.Query{Id = itemId});
            return HandleResult(result);
        }
    }
}
