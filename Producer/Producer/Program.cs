using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Producer;
using Producer.Services;

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<ProducerHostedService>();

        services.AddScoped<MessageBusProducer>();
        services.AddScoped<TranscationSenderService>();

        services.Configure<ProducerOptions>(option => option.Hosts = context.Configuration.GetValue<string>(EnvKeys.KAFKA_BROKERS));
        services.Configure<TransactionsOption>(option => option.KafkaTopic = context.Configuration.GetValue<string>(EnvKeys.KAFKA_TOPIC));
    })
    .Build();

await host.RunAsync();