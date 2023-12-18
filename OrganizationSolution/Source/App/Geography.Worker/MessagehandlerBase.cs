namespace Geography.Worker
{
    using Amazon.SQS.Model;
    using Framework.Business.ServiceProvider.Queue;
    using Framework.Configuration.Models;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public abstract class MessagehandlerBase<TModel> : IHostedService
        where TModel : class
    {
        protected readonly ILogger<MessagehandlerBase<TModel>> _logger;
        protected readonly AmazonSQSConfigurationOptions _amazonSQSConfiguration;
        protected readonly IQueueManager<AmazonSQSConfigurationOptions, List<Message>> _manager;

        public MessagehandlerBase(ILogger<MessagehandlerBase<TModel>> logger, ApplicationOptions options, IQueueManager<AmazonSQSConfigurationOptions, List<Message>> queueManger)
        {
            _logger = logger;
            _amazonSQSConfiguration = options.amazonSQSConfigurationOptions;
            _manager = queueManger;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                List<Message> result = await _manager.ReceiveMessageAsync(_amazonSQSConfiguration);
                if (result != null && result.Count > 0)
                {
                    foreach (var item in result)
                    {
                        var model = JsonConvert.DeserializeObject<TModel>(item.Body);
                        await MessageHandlerAsync(model, cancellationToken);
                        await _manager.DeleteMessageAsync(_amazonSQSConfiguration, item.ReceiptHandle);
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Backgroud service stoped");
            return Task.CompletedTask;
        }

        protected abstract Task MessageHandlerAsync(TModel message, CancellationToken cancellationToken);
    }
}
