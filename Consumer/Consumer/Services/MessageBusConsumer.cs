using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Consumer.Services
{
    public class MessageBusConsumer : IDisposable
    {
        private readonly IConsumer<Null, string> _consumer;
        private readonly ConsumerOptions _options;
        private readonly ILogger _logger;


        public MessageBusConsumer(IOptions<ConsumerOptions> options, ILogger<MessageBusConsumer> logger)
        {
            _options = options.Value;
            _logger = logger;

            var config = new ConsumerConfig { BootstrapServers = _options.Hosts, GroupId = _options.ConsumerGroupId };
            _consumer = new ConsumerBuilder<Null, string>(config).Build();
        }

        public Task ReadAsync<T>(string topicName, Action<T> onMessageReceived)
        {
            var tcs = new TaskCompletionSource();

            _consumer.Subscribe(topicName);

            Task.Run(() =>
            {
                while (true)
                {
                    var result = _consumer.Consume();

                    _logger.LogInformation($"[{_options.ConsumerId}] Read result: Value = {result.Value}, Key = {result.Key}, Partition = {result.TopicPartition}");

                    var message = JsonSerializer.Deserialize<T>(result.Message.Value);

                    onMessageReceived(message);
                }

                tcs.SetResult();
            });

            return tcs.Task;
        }

        public void Dispose()
        {
            _consumer?.Dispose();
        }
    }
}
