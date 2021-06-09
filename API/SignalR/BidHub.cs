
using System;
using System.Threading.Tasks;
using Application.Bid;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class BidHub : Hub
    {
        private readonly IMediator _mediator;
        public BidHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task CreateBid(Create.Command bidModel)
        {
            var bid = await _mediator.Send(bidModel);

            await Clients.Group(bidModel.ItemId.ToString())
                .SendAsync("ReceiveBid", bid.Value);
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var itemId = httpContext.Request.Query["itemId"];
            await Groups.AddToGroupAsync(Context.ConnectionId, itemId);
            var result = await _mediator.Send(new List.Query { ItemId = Guid.Parse(itemId) });
            await Clients.Caller.SendAsync("LoadComments", result.Value);
        }
    }
}
