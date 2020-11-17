// Copyright (c) 2020 Pwn (Jonathan) / All rights reserved.

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using EventSub.Test;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

var hostBuilder = CreateHostBuilder();
var host = hostBuilder.Build();
await host.RunAsync();

static IHostBuilder CreateHostBuilder()
{
    var env = Debugger.IsAttached ? "Development" : "Production";

    IConfiguration config = null;

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

            config = configurationBuilder.Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            Log.Logger.Information($"Initialized for {env}");
        })
        .ConfigureWebHostDefaults(async webHostBuilder =>
        {
            while (config == null) await Task.Delay(TimeSpan.FromMilliseconds(100));

            if (env == "Development")
                webHostBuilder.ConfigureKestrel(options =>
                {
                    options.ListenAnyIP(80);
                    options.ListenAnyIP(443,
                        listenOptions =>
                            listenOptions.UseHttps(config.GetValue<string>("Dev:CertPath")));
                });

            webHostBuilder.UseStartup<Startup>();
        })
        .UseSerilog();

    return host;
}