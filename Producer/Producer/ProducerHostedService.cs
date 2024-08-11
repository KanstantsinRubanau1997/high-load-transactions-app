using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Producer
{
    public class ProducerHostedService : BackgroundService
    {
        private readonly IConfiguration _config;
        private readonly ILogger _logger;


        public ProducerHostedService(
            IConfiguration config,
            ILogger<ProducerHostedService> logger)
        {
            _config = config;
            _logger = logger;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var producerId = _config.GetValue<int>(EnvKeys.PRODUCER_ID_KEY);
            _logger.LogInformation($"Producer {producerId} hosting started");

            using PeriodicTimer timer = new(TimeSpan.FromSeconds(3));
            try
            {
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    _logger.LogInformation("Action");
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Start action is stopped");
            }
        }
    }
}
