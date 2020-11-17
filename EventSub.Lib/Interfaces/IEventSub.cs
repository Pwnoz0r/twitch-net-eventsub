// Copyright (c) 2020 Pwn (Jonathan) / All rights reserved.

using System;
using System.Threading.Tasks;
using EventSub.Lib.Models;
using EventSub.Lib.Models.Responses;

namespace EventSub.Lib.Interfaces
{
    public interface IEventSub
    {
        Task<TwitchEventSubs> GetEventsAsync();

        Task<CreateSubscription> CreateStreamOnlineEventAsync(string channelId, Uri webHookUrl);

        Task DeleteEventAsync(string subscriptionId);
    }
}