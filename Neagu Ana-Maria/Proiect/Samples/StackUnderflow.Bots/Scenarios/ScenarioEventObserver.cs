using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Orleans.Streams;

namespace StackUnderflow.Bots.Scenarios
{
    public class ScenarioEventObserver : IAsyncObserver<object>
    {
        private readonly ScenarioGrain scenario;

        public ScenarioEventObserver(ScenarioGrain scenario)
        {
            this.scenario = scenario;
        }

        public async Task OnNextAsync(object item, StreamSequenceToken token = null)
        {
            await scenario.On(item);
        }

        public Task OnCompletedAsync()
        {
            return Task.CompletedTask;
        }

        public Task OnErrorAsync(Exception ex)
        {
            return Task.CompletedTask;
        }
    }
}
