// Copyright (c) 2020 Pwn (Jonathan) / All rights reserved.

using System;
using System.Threading.Tasks;
using EventSub.Lib.Enums;
using EventSub.Lib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EventSub.Test.Controllers.Api
{
    [Route("api/[controller]")]
    public class TwitchController : ControllerBase
    {
        private readonly ILogger<TwitchController> _logger;

        public TwitchController(ILogger<TwitchController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("stream")]
        public async Task<IActionResult> StreamStatus([FromBody] CallbackVerify verify)
        {
            switch (verify.Subscription.StatusType)
            {
                case EventSubStatusType.None: // Unable to parse status
                    break;
                case EventSubStatusType.Enabled:
                    break;
                case EventSubStatusType.Pending:
                    // TODO: Verify status
                    break;
                case EventSubStatusType.Failed:
                    break;
                case EventSubStatusType.FailuresExceeded:
                    break;
                case EventSubStatusType.Revoked:
                    break;
                case EventSubStatusType.Removed:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return Ok(verify.Challenge);
        }
    }
}