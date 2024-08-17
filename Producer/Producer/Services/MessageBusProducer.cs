using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Producer.Services
{
    public class MessageBusProducer : IDisposable
    {
        private readonly IProducer<Null, string> _producer;
        private readonly ProducerOptions _options;
        private readonly ILogger _logger;


        public MessageBusProducer(IOptions<ProducerOptions> options, ILogger<MessageBusProducer> logger)
        {
            _options = options.Value;
            _logger = logger;

            var config = new ProducerConfig { BootstrapServers = _options.Hosts };
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task WriteAsync<T>(string topicName, T message)
        {
            var messageJson = JsonSerializer.Serialize(message);
            var result = await _producer.ProduceAsync(topicName, new Message<Null, string> { Value = messageJson });

            _logger.LogInformation($"Send result: Value = {result.Value}, Key = {result.Key}, Partition = {result.TopicPartition}");
        }

        public void Dispose()
        {
            _producer?.Dispose();
        }
    }
}
