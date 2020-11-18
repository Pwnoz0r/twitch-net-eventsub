// Copyright (c) 2020 Pwn (Jonathan) / All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace EventSub.Lib.Models.Responses
{
    public class CreateSubscription
    {
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public List<Datum> Data { get; set; }

        [JsonProperty("limit", NullValueHandling = NullValueHandling.Ignore)]
        public long? Limit { get; set; }

        [JsonProperty("total", NullValueHandling = NullValueHandling.Ignore)]
        public long? Total { get; set; }
    }

    public class Datum
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

        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedAt { get; set; }

        [JsonProperty("transport", NullValueHandling = NullValueHandling.Ignore)]
        public Transport Transport { get; set; }
    }
}