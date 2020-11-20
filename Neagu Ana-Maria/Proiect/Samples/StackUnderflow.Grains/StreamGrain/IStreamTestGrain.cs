using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Orleans;
using Orleans.Streams;

namespace StackUnderflow.Backend.Grains.StreamGrain
{
    public interface IStreamTestGrain : IGrainWithGuidCompoundKey
    {
        Task Start();
    }

    public class StreamTestGrain : Grain, IStreamTestGrain, IAsyncObserver<string>
    {
        public override async Task OnActivateAsync()
        {
            var stream = this.GetStreamProvider("RedisProvider").GetStream<string>(Guid.Empty, "test");
            await stream.SubscribeAsync(this);
        }

        public Task Start()
        {
            return Task.CompletedTask;
        }

        public Task OnNextAsync(string item, StreamSequenceToken token = null)
        {
            Console.WriteLine(item);
            return Task.CompletedTask;
        }

        public Task OnCompletedAsync()
        {
            throw new NotImplementedException();
        }

        public Task OnErrorAsync(Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
