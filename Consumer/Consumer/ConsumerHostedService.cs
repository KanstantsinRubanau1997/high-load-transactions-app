using Consumer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Consumer
{
    public class ConsumerHostedService : BackgroundService
    {
        private readonly IConfiguration _config;
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;


        public ConsumerHostedService(
            IConfiguration config,
            ILogger<ConsumerHostedService> logger,
            IServiceScopeFactory serviceScopeFactory) 
        {
            _logger = logger;
            _config = config;
            _serviceScopeFactory = serviceScopeFactory;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumerId = _config.GetValue<int>(EnvKeys.CONCUMER_ID_KEY);
            _logger.LogInformation($"Consumer {consumerId} hosting started");

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var transactionsReader = scope.ServiceProvider.GetRequiredService<TransactionReaderService>();

                await transactionsReader.ReadTransactionsAsync();
            }
        }
    }
}
