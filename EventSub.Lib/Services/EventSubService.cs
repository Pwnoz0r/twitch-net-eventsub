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

        public async Task<TwitchEventSubs> GetEventsAsync()
        {
            var httpClient = await GenerateEventSubHttpClient();
            if (httpClient == default) return default;

            var response = await httpClient.GetAsync(Shared.TwitchEventSubSubscriptionsEndpoint);

            TwitchEventSubs twitchEventSubs = null;

            while (twitchEventSubs == null) twitchEventSubs = await ParseResponse<TwitchEventSubs>(response);

            return twitchEventSubs;
        }

        public async Task CreateEventAsync()
        {
            var httpClient = await GenerateEventSubHttpClient();
            if (httpClient == default) return;

            //var response = httpClient.PostAsync(Shared.TwitchEventSubSubscriptionsEndpoint)
        }

        private async Task AuthorizeAsync()
        {
            while (true)
            {
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
                    _logger.LogWarning($"Unable to authorize client: {responseContent.Trim()}");

                    _authRetryCounter++;

                    if (_authRetryCounter >= _maxRetries) return;

                    await Task.Delay(TimeSpan.FromSeconds(3));
                    continue;
                }

                _twitchClientToken = JsonConvert.DeserializeObject<TwitchClientToken>(responseContent);
                _authRetryCounter = 0;

                break;
            }
        }

        private async Task<HttpClient> GenerateEventSubHttpClient()
        {
            if (_twitchClientToken == null)
            {
                await AuthorizeAsync();
                return default;
            }

            var httpClient = new HttpClient {BaseAddress = Shared.TwitchEventSubBaseUri};
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("Client-ID", _clientId);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_twitchClientToken.AccessToken}");

            return httpClient;
        }

        private async Task<T> ParseResponse<T>(HttpResponseMessage response)
        {
            if (response?.RequestMessage == null) return default;

            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    $"Failed Response [{response.RequestMessage.Method}] -> {response.RequestMessage.RequestUri} -> {responseContent.Trim()}");

                _retryCounter++;

                if (_retryCounter >= _maxRetries)
                {
                    _retryCounter = 0;
                    return default;
                }

                await Task.Delay(TimeSpan.FromSeconds(3));
            }

            _retryCounter = 0;
            return JsonConvert.DeserializeObject<T>(responseContent);
        }
    }
}