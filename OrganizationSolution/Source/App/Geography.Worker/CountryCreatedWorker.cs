namespace Geography.Worker
{
    using Amazon.SQS.Model;
    using Framework.Business;
    using Framework.Business.ServiceProvider.Queue;
    using Framework.Configuration.Models;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Geography.Business.Country.Models;
    using Framework.Business.Extension;

    public class CountryCreatedWorker : MessagehandlerBase<List<IndexedItem<CountryCreateModel>>>
    {
        public CountryCreatedWorker(ILogger<CountryCreatedWorker> logger, ApplicationOptions applicationOptions, IQueueManager<AmazonSQSConfigurationOptions, List<Message>> queueManger)
            : base(logger, applicationOptions, queueManger)
        {
        }

        protected override Task MessageHandlerAsync(List<IndexedItem<CountryCreateModel>> message, CancellationToken cancellationToken)
        {
            _logger.LogInformation("message received {0}", JsonConvert.SerializeObject(message));
            return Task.CompletedTask;
        }
    }
}
