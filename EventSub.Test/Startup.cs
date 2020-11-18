// Copyright (c) 2020 Pwn (Jonathan) / All rights reserved.

using EventSub.Lib.Interfaces;
using EventSub.Lib.Services;
using EventSub.Test.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace EventSub.Test
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IEventSub, EventSubService>();

            services.AddHostedService<TestService>();

            services.AddRouting(x =>
            {
                x.LowercaseUrls = true;
                x.LowercaseQueryStrings = true;
            });

            services.AddControllers().AddNewtonsoftJson();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                context.Request.EnableBuffering();
                await next();
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
        }
    }
}