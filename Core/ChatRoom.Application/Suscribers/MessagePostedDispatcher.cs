﻿using ChatRoom.Application.Events;
using ChatRoom.ComService;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatRoom.Application.Suscribers
{
    public class MessagePostedDispatcher : INotificationHandler<MessagePostedEvent>
    {
        private readonly IHubContext<ChatRoomHub> _hubContext;
        private readonly ILogger _logger;

        public MessagePostedDispatcher(ILogger<MessagePostedDispatcher> logger,
            IHubContext<ChatRoomHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }


        Task INotificationHandler<MessagePostedEvent>.Handle(MessagePostedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($" [x] Sent to SignalR: {notification.Message} from notification handler");

            return _hubContext.Clients.All.SendAsync("Send", notification, cancellationToken);
        }
    }
}
