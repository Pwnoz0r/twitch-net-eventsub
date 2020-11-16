// Copyright (c) 2020 Pwn (Jonathan) / All rights reserved.

using System;

namespace EventSub.Lib
{
    public class Shared
    {
        public static Uri TwitchEventSubBaseUri { get; } = new("https://api.twitch.tv/helix/eventsub/subscriptions");

        public static Uri TwitchAuthUri { get; } = new("https://id.twitch.tv/oauth2/token");
    }
}