// Copyright (c) 2020 Pwn (Jonathan) / All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace EventSub.Lib.Models
{
    public class TwitchClientToken
    {
        [JsonProperty("access_token", NullValueHandling = NullValueHandling.Ignore)]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in", NullValueHandling = NullValueHandling.Ignore)]
        public long? ExpiresIn { get; set; }

        [JsonProperty("scope", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Scope { get; set; }

        [JsonProperty("token_type", NullValueHandling = NullValueHandling.Ignore)]
        public string TokenType { get; set; }
    }
}