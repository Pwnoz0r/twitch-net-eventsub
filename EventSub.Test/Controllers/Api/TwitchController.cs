// Copyright (c) 2020 Pwn (Jonathan) / All rights reserved.

using System.Threading.Tasks;
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
            // TODO: Verify request is legit
            return Ok(verify.Challenge);
        }
    }
}