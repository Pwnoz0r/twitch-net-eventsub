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
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly IConfiguration _config;
        private readonly ILogger<EventSubService> _logger;
        private readonly int _maxRetries;

        private int _authRetryCounter;
        private int _retryCounter;
        private TwitchClientToken _twitchClientToken;

        public EventSubService(ILogger<EventSubService> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _maxRetries = _config.GetValue<int>("EventSub:MaxRetries");
            _clientId = _config.GetValue<string>("EventSub:ClientId");
            _clientSecret = _config.GetValue<string>("EventSub:ClientSecret");
        }

        public async Task<bool> AuthorizeAsync()
        {
            if (_authRetryCounter >= _maxRetries)
                return false;

            var queryBuilder = new QueryBuilder
            {
                {"client_id", _clientId},
                {"client_secret", _clientSecret},
                {"grant_type", "client_credentials"},
                {"scope", "user:edit"}
            };

            var uriBuilder = new UriBuilder(Shared.TwitchAuthUri) {Query = queryBuilder.ToString()};

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();

            var response = await httpClient.PostAsync(uriBuilder.Uri, default!);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning($"Unable to authorize client: {responseContent}");

                _authRetryCounter++;
                await Task.Delay(TimeSpan.FromSeconds(3));
                await AuthorizeAsync();
            }

            _twitchClientToken = JsonConvert.DeserializeObject<TwitchClientToken>(responseContent);
            return true;
        }

        public async Task<TwitchEventSubs> GetEventsAsync()
        {
            if (_retryCounter >= _maxRetries)
                return default;

            var httpClient = await GenerateEventSubHttpClient();

            var response = await httpClient.GetAsync("subscriptions");
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode) return JsonConvert.DeserializeObject<TwitchEventSubs>(responseContent);

            _logger.LogWarning($"Unable to get events: {responseContent}");

            _retryCounter++;
            await GetEventsAsync();
            await Task.Delay(TimeSpan.FromSeconds(3));

            return JsonConvert.DeserializeObject<TwitchEventSubs>(responseContent);
        }

        public Task CreateEventAsync()
        {
            throw new NotImplementedException();
        }

        private async Task<HttpClient> GenerateEventSubHttpClient()
        {
            if (_twitchClientToken == null) await AuthorizeAsync();

            var httpClient = new HttpClient {BaseAddress = Shared.TwitchEventSubBaseUri};
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("Client-ID", _clientId);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_twitchClientToken.AccessToken}");

            return httpClient;
        }
    }
}