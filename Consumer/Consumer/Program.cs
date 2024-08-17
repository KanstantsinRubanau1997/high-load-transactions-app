using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Consumer;
using Consumer.Services;

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<ConsumerHostedService>();

        services.AddScoped<MessageBusConsumer>();
        services.AddScoped<TransactionReaderService>();

        services.Configure<ConsumerOptions>(option =>
        {
            option.Hosts = context.Configuration.GetValue<string>(EnvKeys.KAFKA_BROKERS);
            option.ConsumerGroupId = context.Configuration.GetValue<string>(EnvKeys.KAFKA_CONSUMER_GROUP_ID);
            option.ConsumerId = context.Configuration.GetValue<string>(EnvKeys.CONCUMER_ID_KEY);
        });
        services.Configure<TransactionsOption>(option => option.KafkaTopic = context.Configuration.GetValue<string>(EnvKeys.KAFKA_TOPIC));
    })
    .Build();

await host.RunAsync();