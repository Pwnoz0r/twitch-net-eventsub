using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventSub.Lib.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EventSub.Test.Services
{
    public class TestService : BackgroundService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<TestService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public TestService(ILogger<TestService> logger, IConfiguration config, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _config = config;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var scope = _serviceProvider.CreateScope();
            var eventSub = scope.ServiceProvider.GetService<IEventSub>();

            if (eventSub == null)
            {
                _logger.LogWarning("Event Sub Service is Null");
                return;
            }

            var events = await eventSub.GetEventsAsync();

            _logger.LogDebug($"Got {events.Data.Count} EventSubs");

            if (!events.Data.Any())
            {
                var streamOnline = await eventSub.CreateStreamOnlineEventAsync(
                    _config.GetValue<string>("EventSub:StreamOnline:ChannelId"),
                    _config.GetValue<Uri>("EventSub:StreamOnline:WebHookUrl"));

                var tempEvent = events.Data.FirstOrDefault(x => x.Id == streamOnline.Data.First().Id);

                while (true)
                {
                    events = await eventSub.GetEventsAsync();
                    tempEvent = events.Data.First(x => x.Id == streamOnline.Data.First().Id);

                    if (tempEvent != default) _logger.LogDebug($"[{tempEvent.Id}]: {tempEvent.Status}");

                    await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
                }
            }

            foreach (var twitchEventSub in events.Data)
                _logger.LogDebug($"[{twitchEventSub.Id}]: {twitchEventSub.Status}");
            await eventSub.DeleteEventAsync(events.Data.First().Id);
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting Test Service");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogWarning("Stopping Test Service");
            return base.StopAsync(cancellationToken);
        }
    }
}