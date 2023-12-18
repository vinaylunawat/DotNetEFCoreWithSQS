using Amazon.SQS.Model;
using Framework.Business.ServiceProvider.Queue;
using Framework.Configuration;
using Framework.Configuration.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace Geography.Worker
{
    /// <summary>
    /// Program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        async static Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                .Build();

            var hostBuilder = new HostBuilder().ConfigureServices(services =>
            {
                services.AddHostedService<SQSMsgSubscriberBackgroudService>();
                services.Configure<AmazonSQSConfigurationOptions>(configuration.GetSection(nameof(AmazonSQSConfigurationOptions)));
                services.AddSingleton(typeof(IQueueManager<AmazonSQSConfigurationOptions, List<Message>>), typeof(QueueManager));

            }).DefaultAppConfiguration(new[] { typeof(ApplicationOptions).Assembly, typeof(SecurityOptions).Assembly }, args);
            var host = hostBuilder.UseConsoleLifetime().Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var serviceBusProvider = services.GetRequiredService<IQueueManager<AmazonSQSConfigurationOptions, List<Message>>>();
                var applicationOptions = services.GetRequiredService<ApplicationOptions>();
                await serviceBusProvider.Initialize(applicationOptions.amazonSQSConfigurationOptions);
            }
            await host.RunAsync();
        }
    }
}
