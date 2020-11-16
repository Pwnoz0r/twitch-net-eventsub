// Copyright (c) 2020 Pwn (Jonathan) / All rights reserved.

using System;
using System.Net.Http;
using System.Threading.Tasks;
using EventSub.Lib.Interfaces;
using EventSub.Lib.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EventSub.Lib.Services
{
    public class EventSubService : IEventSub
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EventSubService> _logger;
        private readonly int _maxRetries;

        private int _retryCounter;
        private TwitchClientToken _twitchClientToken;

        public EventSubService(ILogger<EventSubService> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _maxRetries = _config.GetValue<int>("Twitch:MaxRetries");
        }

        public async Task AuthorizeAsync()
        {
            var queryBuilder = new QueryBuilder
            {
                {"client_id", _config.GetValue<string>("Twitch:ClientId")},
                {"client_secret", _config.GetValue<string>("Twitch:ClientSecret")},
                {"grant_type", "client_credentials"},
                {"scope", "user:edit"}
            };

            var uriBuilder = new UriBuilder(Shared.TwitchAuthUri) {Query = queryBuilder.ToString()};

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();

            var response = await httpClient.PostAsync(uriBuilder.Uri, default!);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                _logger.LogWarning($"Unable to authorize client: {responseContent}");
            else
                _twitchClientToken = JsonConvert.DeserializeObject<TwitchClientToken>(responseContent);
        }

        public async Task<TwitchEventSubs> GetEventsAsync()
        {
            if (_twitchClientToken == null) await AuthorizeAsync();

            var httpClient = GenerateEventSubHttpClient();

            var response = await httpClient.GetAsync("subscriptions");
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode) return JsonConvert.DeserializeObject<TwitchEventSubs>(responseContent);

            if (_retryCounter >= _maxRetries)
                return default;

            _logger.LogWarning($"Unable to get events: {responseContent}");
            await AuthorizeAsync();
            await GetEventsAsync();
            await Task.Delay(TimeSpan.FromSeconds(3));
            _retryCounter++;

            return JsonConvert.DeserializeObject<TwitchEventSubs>(responseContent);
        }

        private HttpClient GenerateEventSubHttpClient()
        {
            var httpClient = new HttpClient {BaseAddress = Shared.TwitchEventSubBaseUri};
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("Client-ID", _config.GetValue<string>("Twitch:ClientId"));
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_twitchClientToken.AccessToken}");

            return httpClient;
        }
    }
}