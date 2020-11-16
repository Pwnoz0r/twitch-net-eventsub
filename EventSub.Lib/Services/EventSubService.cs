// Copyright (c) 2020 Pwn (Jonathan) / All rights reserved.

using EventSub.Lib.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EventSub.Lib.Services
{
    public class EventSubService : IEventSub
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EventSubService> _logger;

        public EventSubService(ILogger<EventSubService> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }
    }
}