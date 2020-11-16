// Copyright (c) 2020 Pwn (Jonathan) / All rights reserved.

using System;
using Newtonsoft.Json;

namespace EventSub.Lib.Models.Responses
{
    public class StreamOnlineNotification
    {
        [JsonProperty("subscription", NullValueHandling = NullValueHandling.Ignore)]
        public Subscription Subscription { get; set; }

        [JsonProperty("event", NullValueHandling = NullValueHandling.Ignore)]
        public Event Event { get; set; }
    }

    public class Event
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("broadcaster_user_id", NullValueHandling = NullValueHandling.Ignore)]
        public string BroadcasterUserId { get; set; }

        [JsonProperty("broadcaster_user_name", NullValueHandling = NullValueHandling.Ignore)]
        public string BroadcasterUserName { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
    }

    public class Subscription
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("version", NullValueHandling = NullValueHandling.Ignore)]
        public string Version { get; set; }

        [JsonProperty("condition", NullValueHandling = NullValueHandling.Ignore)]
        public Condition Condition { get; set; }

        [JsonProperty("transport", NullValueHandling = NullValueHandling.Ignore)]
        public Transport Transport { get; set; }

        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedAt { get; set; }
    }

    public class Condition
    {
        [JsonProperty("broadcaster_user_id", NullValueHandling = NullValueHandling.Ignore)]
        public string BroadcasterUserId { get; set; }
    }

    public class Transport
    {
        [JsonProperty("method", NullValueHandling = NullValueHandling.Ignore)]
        public string Method { get; set; }

        [JsonProperty("callback", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Callback { get; set; }
    }
}