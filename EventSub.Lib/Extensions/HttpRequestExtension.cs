// Copyright (c) 2020 Pwn (Jonathan) / All rights reserved.

using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EventSub.Lib.Extensions
{
    public static class HttpRequestExtension
    {
        public static async Task<string> GetBodyAsStringAsync(this HttpRequest request, Encoding encoding = null)
        {
            request.Body.Position = 0;

            encoding ??= Encoding.UTF8;

            using var reader = new StreamReader(request.Body, encoding);
            return await reader.ReadToEndAsync();
        }

        public static async Task<byte[]> GetBodyAsBytesAsync(this HttpRequest request)
        {
            request.Body.Position = 0;
            await using var ms = new MemoryStream(2048);
            await request.Body.CopyToAsync(ms);
            return ms.ToArray();
        }
    }
}