using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Access.Primitives.Extensions.Cloning;
using Access.Primitives.IO;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Streams;


namespace Access.Primitives.Orleans.Streaming.Adapters
{
    public struct PublishCmd
    {
        public string Provider { get; }
        public Guid Partition { get; }
        public string Topic { get; }
        public IEnumerable<object> Events { get; }

        public PublishCmd(string provider, Guid partition, string topic, params object[] @events)
        {
            Provider = provider;
            Partition = partition;
            Topic = topic;
            Events = @events;
        }
    }

    public class PublishResult { }


    public class PublishAdapter : Adapter<PublishCmd, PublishResult>
    {
        private readonly IServiceProvider _serviceProvider;

        public PublishAdapter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override Task PostConditions(PublishCmd cmd, PublishResult result, object state) => Task.CompletedTask;

        public override async Task<PublishResult> Work(PublishCmd cmd, object state, object dependencies)
        {
            var streamProvider = GetStream(cmd.Provider, cmd.Partition, cmd.Topic);
            await streamProvider.OnNextBatchAsync(cmd.Events);
            return new PublishResult();
        }


        private IAsyncStream<object> GetStream(string streamProvider, Guid partition, string topic)
        {
            return _serviceProvider.GetService<IClusterClient>().GetStreamProvider(streamProvider)
                .GetStream<object>(partition, topic);
        }
    }
}
