using System;
using MediatR;

namespace Application.Notifications.Models
{
    public class ItemDeletedNotification : INotification
    {
        public ItemDeletedNotification(Guid itemId)
        {
            this.ItemId = itemId;
        }

        public Guid ItemId { get; set; }
    }
}