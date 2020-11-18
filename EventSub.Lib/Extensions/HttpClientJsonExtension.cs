// Copyright (c) 2020 Pwn (Jonathan) / All rights reserved.

#nullable enable
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EventSub.Lib.Extensions
{
    public static class HttpClientJsonExtensions
    {
        public static Task<HttpResponseMessage> PostAsNewtonsoftJsonAsync<TValue>(this HttpClient client,
            string? requestUri, TValue value, JsonSerializerOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            var content = new StringContent(JsonConvert.SerializeObject(value, Formatting.None), Encoding.UTF8,
                "application/json");

            return client.PostAsync(requestUri, content, cancellationToken);
        }

        public static Task<HttpResponseMessage> PostAsNewtonsoftJsonAsync<TValue>(this HttpClient client,
            Uri? requestUri, TValue value, JsonSerializerOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            var content = new StringContent(JsonConvert.SerializeObject(value, Formatting.None), Encoding.UTF8,
                "application/json");
            return client.PostAsync(requestUri, content, cancellationToken);
        }

        public static Task<HttpResponseMessage> PostAsNewtonsoftJsonAsync<TValue>(this HttpClient client,
            string? requestUri, TValue value, CancellationToken cancellationToken)
        {
            return client.PostAsNewtonsoftJsonAsync(requestUri, value, null, cancellationToken);
        }

        public static Task<HttpResponseMessage> PostAsNewtonsoftJsonAsync<TValue>(this HttpClient client,
            Uri? requestUri, TValue value, CancellationToken cancellationToken)
        {
            return client.PostAsNewtonsoftJsonAsync(requestUri, value, null, cancellationToken);
        }
    }
}