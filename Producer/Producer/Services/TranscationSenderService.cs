using Microsoft.Extensions.Options;
using Producer.Services.DataContracts;

namespace Producer.Services
{
    public class TranscationSenderService
    {
        private readonly MessageBusProducer _messageBusProducer;
        private readonly TransactionsOption _options;


        public TranscationSenderService(MessageBusProducer messageBusProducer, IOptions<TransactionsOption> options)
        {
            _messageBusProducer = messageBusProducer;
            _options = options.Value;
        }


        public async Task ProduceRandomTransaction()
        {
            var someClientId = Guid.NewGuid().ToString();

            var random = new Random();
            var randomTransaction = random.NextDouble() * 100 * (random.Next(0, 1) > 0.5 ? -1 : 1);

            var transaction = new TransactionDataContract { ClientId = someClientId, Value = randomTransaction };
            await _messageBusProducer.WriteAsync(_options.KafkaTopic, transaction);
        }
    }
}
