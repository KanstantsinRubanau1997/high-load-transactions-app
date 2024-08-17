using Consumer.Services.DataContracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Consumer.Services
{
    public class TransactionReaderService
    {
        private readonly MessageBusConsumer _consumer;
        private readonly TransactionsOption _option;
        private readonly ILogger _logger;


        public TransactionReaderService(
            MessageBusConsumer consumer,
            IOptions<TransactionsOption> option,
            ILogger<TransactionReaderService> logger) 
        {
            _consumer = consumer;
            _option = option.Value;
            _logger = logger;
        }


        public async Task ReadTransactionsAsync()
        {
            await _consumer.ReadAsync<TransactionDataContract>(_option.KafkaTopic, transaction =>
            {
                _logger.LogInformation($"Processing transaction for {transaction.ClientId} with value {transaction.Value}");
            });
        }
    }
}
