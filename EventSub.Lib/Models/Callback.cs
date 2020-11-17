// Copyright (c) 2020 Pwn (Jonathan) / All rights reserved.

using EventSub.Lib.Attributes;
using EventSub.Lib.Enums;
using EventSub.Lib.Extensions;
using Newtonsoft.Json;

namespace EventSub.Lib.Models
{
    public class Callback
    {
        [JsonProperty("subscription", NullValueHandling = NullValueHandling.Ignore)]
        public Subscription Subscription { get; set; }

        [JsonProperty("event", NullValueHandling = NullValueHandling.Ignore)]
        public Event Event { get; set; }

        [JsonProperty("challenge", NullValueHandling = NullValueHandling.Ignore)]
        public string Challenge { get; set; }
    }

    public class Event
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
        public string UserId { get; set; }

        [JsonProperty("user_name", NullValueHandling = NullValueHandling.Ignore)]
        public string UserName { get; set; }

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

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        public EventSubStatusType StatusType =>
            Status.GetValueFromDescription<EventSubStatusType, EventSubStatusAttribute>("Status");

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
}