// Copyright (c) 2020 Pwn (Jonathan) / All rights reserved.

using System.Threading.Tasks;
using EventSub.Lib.Models;

namespace EventSub.Lib.Interfaces
{
    public interface IEventSub
    {
        Task<TwitchEventSubs> GetEventsAsync();

        Task CreateEventAsync();
    }
}