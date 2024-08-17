using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Producer.Services;

namespace Producer
{
    public class ProducerHostedService : BackgroundService
    {
        private readonly IConfiguration _config;
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;


        public ProducerHostedService(
            IConfiguration config,
            ILogger<ProducerHostedService> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _config = config;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var producerId = _config.GetValue<int>(EnvKeys.PRODUCER_ID_KEY);
            _logger.LogInformation($"Producer {producerId} hosting started");

            using(var scope = _serviceScopeFactory.CreateScope())
            {
                var transactionsSender = scope.ServiceProvider.GetRequiredService<TranscationSenderService>();

                using PeriodicTimer timer = new(TimeSpan.FromSeconds(1));
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    await transactionsSender.ProduceRandomTransaction();
                }
            }
        }
    }
}
