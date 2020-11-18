// Copyright (c) 2020 Pwn (Jonathan) / All rights reserved.

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EventSub.Lib
{
    public class Shared
    {
        public static Uri TwitchEventSubBaseUri { get; } = new("https://api.twitch.tv/helix/eventsub/subscriptions");

        public static Uri TwitchAuthUri { get; } = new("https://id.twitch.tv/oauth2/token");

        public static string TwitchEventSubSubscriptionsEndpoint { get; } = "subscriptions";

        public static JsonSerializerSettings SerializerMinified { get; } = new()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.None,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore
        };
    }
}