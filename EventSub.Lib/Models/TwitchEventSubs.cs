// Copyright (c) 2020 Pwn (Jonathan) / All rights reserved.

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace EventSub.Lib.Models
{
    public class TwitchEventSubs
    {
        [JsonProperty("total", NullValueHandling = NullValueHandling.Ignore)]
        public long? Total { get; set; }

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public List<TwitchEventSub> Data { get; set; }
    }

    public class TwitchEventSub
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

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

        [JsonProperty("secret", NullValueHandling = NullValueHandling.Ignore)]
        public string Secret { get; set; }
    }
}