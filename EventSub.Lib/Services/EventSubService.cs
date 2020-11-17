// Copyright (c) 2020 Pwn (Jonathan) / All rights reserved.

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using EventSub.Lib.Attributes;
using EventSub.Lib.Enums;
using EventSub.Lib.Extensions;
using EventSub.Lib.Interfaces;
using EventSub.Lib.Models;
using EventSub.Lib.Models.Responses;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Condition = EventSub.Lib.Models.Condition;
using Transport = EventSub.Lib.Models.Transport;

namespace EventSub.Lib.Services
{
    public class EventSubService : IEventSub
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly ILogger<EventSubService> _logger;
        private readonly int _maxRetries;

        private int _authRetryCounter;
        private int _retryCounter;
        private TwitchClientToken _twitchClientToken;

        public EventSubService(ILogger<EventSubService> logger, IConfiguration config)
        {
            _logger = logger;
            _maxRetries = config.GetValue<int>("EventSub:MaxRetries");
            _clientId = config.GetValue<string>("EventSub:ClientId");
            _clientSecret = config.GetValue<string>("EventSub:ClientSecret");
        }

        public async Task<TwitchEventSubs> GetEventsAsync()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var httpClient = await GenerateEventSubHttpClient();
            if (httpClient == default) return default;

            TwitchEventSubs twitchEventSubs = null;

            while (!cancellationTokenSource.IsCancellationRequested)
            {
                var response = await httpClient.GetAsync(Shared.TwitchEventSubSubscriptionsEndpoint,
                    cancellationTokenSource.Token);
                twitchEventSubs = await ParseResponse<TwitchEventSubs>(response, cancellationTokenSource);
            }

            return twitchEventSubs;
        }

        public async Task<CreateSubscription> CreateStreamOnlineEventAsync(string channelId, Uri webHookUrl)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var httpClient = await GenerateEventSubHttpClient();
            if (httpClient == default) return default;

            const EventSubType eventType = EventSubType.StreamOnline;
            var eventData = eventType.GetAttributeOfType<EventSubTypeAttribute>();

            var secret = Guid.NewGuid();

            var streamOnlineEvent = new TwitchEventSub
            {
                Type = eventData.Type,
                Version = "1",
                Condition = new Condition
                {
                    BroadcasterUserId = channelId
                },
                Transport = new Transport
                {
                    Method = "webhook",
                    Callback = webHookUrl,
                    Secret = secret.ToString("N")
                }
            };

            CreateSubscription streamOnline = null;

            while (!cancellationTokenSource.IsCancellationRequested)
            {
                var response = await httpClient.PostAsNewtonsoftJsonAsync(Shared.TwitchEventSubSubscriptionsEndpoint,
                    streamOnlineEvent, cancellationTokenSource.Token);
                streamOnline = await ParseResponse<CreateSubscription>(response, cancellationTokenSource);
            }

            return streamOnline;
        }

        public async Task DeleteEventAsync(string subscriptionId)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var httpClient = await GenerateEventSubHttpClient();
            if (httpClient == default) return;

            var queryBuilder = new QueryBuilder
            {
                {"id", subscriptionId}
            };

            var uriBuilder = new UriBuilder(Shared.TwitchEventSubBaseUri) {Query = queryBuilder.ToString()};

            var response = await httpClient.DeleteAsync(uriBuilder.ToString(), cancellationTokenSource.Token);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogDebug($"Successfully deleted event: {subscriptionId}");
            }
            else
            {
                var responseString = await response.Content.ReadAsStringAsync(cancellationTokenSource.Token);
                _logger.LogWarning($"Failed to delete {subscriptionId}: [{response.StatusCode}] {responseString}");
            }
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
                if (_twitchClientToken == null)
                    return default;
            }

            var httpClient = new HttpClient {BaseAddress = Shared.TwitchEventSubBaseUri};
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("Client-ID", _clientId);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_twitchClientToken.AccessToken}");

            return httpClient;
        }

        private async Task<T> ParseResponse<T>(HttpResponseMessage response, CancellationTokenSource cancellationToken)
        {
            if (response?.RequestMessage == null) return default;

            if (response.RequestMessage.Content != null)
            {
                var requestContent = await response.RequestMessage.Content.ReadAsStringAsync();
                _logger.LogDebug(
                    $"[{response.RequestMessage.Method}] -> {response.RequestMessage.RequestUri}: {requestContent.Trim()}");
            }

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken.Token);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    $"Failed Response [{response.RequestMessage.Method}] -> {response.RequestMessage.RequestUri} -> {responseContent.Trim()}");

                _retryCounter++;

                if (_retryCounter >= _maxRetries)
                {
                    cancellationToken.Cancel(true);
                    return default;
                }

                await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken.Token);
                return default;
            }

            _retryCounter = 0;
            cancellationToken.Cancel(true);
            return JsonConvert.DeserializeObject<T>(responseContent);
        }
    }
}