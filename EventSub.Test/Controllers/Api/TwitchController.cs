// Copyright (c) 2020 Pwn (Jonathan) / All rights reserved.

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using EventSub.Lib.Enums;
using EventSub.Lib.Extensions;
using EventSub.Lib.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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

        [HttpGet]
        [Route("stream")]
        public async Task<IActionResult> GetStream()
        {
            return Ok();
        }

        [HttpPost]
        [Route("stream")]
        public async Task<IActionResult> PostStream([FromBody] Callback callback)
        {
            switch (callback.Subscription.StatusType)
            {
                case EventSubStatusType.None: // Unable to parse status
                    break;
                case EventSubStatusType.Enabled:
                    break;
                case EventSubStatusType.Pending:
                    // TODO: Verify status
                    return Ok(callback.Challenge);
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

            _logger.LogDebug($"{callback.Subscription.Id} -> [{callback.Event.BroadcasterUserName} ({callback.Event.BroadcasterUserId}): {callback.Event.Type}");

            return Ok();
        }
    }
}