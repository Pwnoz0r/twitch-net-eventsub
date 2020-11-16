// Copyright (c) 2020 Pwn (Jonathan) / All rights reserved.

using System.Diagnostics;
using System.IO;
using System.Reflection;
using EventSub.Lib.Interfaces;
using EventSub.Lib.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

static IHostBuilder CreateHostBuilder()
{
    var env = Debugger.IsAttached ? "Development" : "Production";

    var host = Host.CreateDefaultBuilder()
        .UseEnvironment(env)
        .ConfigureAppConfiguration((hostContext, configurationBuilder) =>
        {
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
            configurationBuilder.AddJsonFile("appsettings.json", false, true);
            configurationBuilder.AddJsonFile("appsettings-production.json", false, true);
            configurationBuilder.AddEnvironmentVariables();

            if (hostContext.HostingEnvironment.IsDevelopment())
            {
                var appAssembly =
                    Assembly.Load(new AssemblyName(Assembly.GetExecutingAssembly().FullName ?? string.Empty));

                configurationBuilder.AddUserSecrets(appAssembly, true);
            }

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configurationBuilder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            Log.Logger.Information($"Initialized for {env}");
        })
        .ConfigureServices((_, services) => { services.AddScoped<IEventSub, EventSubService>(); })
        .UseSerilog();

    return host;
}