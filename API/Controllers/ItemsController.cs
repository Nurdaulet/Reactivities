using System.Threading.Tasks;
using Application.Items;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ItemsController: BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetItems([FromQuery] ItemParams param)
        {
            return HandlePagedResult(await Mediator.Send(new List.Query { Params = param }));
        }
    }
}