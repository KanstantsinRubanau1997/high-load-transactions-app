using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Producer;

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((_, services) =>
    {
        services.AddHostedService<ProducerHostedService>();
    })
    .Build();

await host.RunAsync();