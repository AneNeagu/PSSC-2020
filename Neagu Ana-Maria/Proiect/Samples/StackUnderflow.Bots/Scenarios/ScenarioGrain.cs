using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;
using Orleans.Streams;

namespace StackUnderflow.Bots.Scenarios
{
    public interface IScenarioGrain : IGrainWithGuidKey
    {
        Task ExecuteAsync();
        Task RegisterSubscription<TStream>(Guid streamId);
    }

    public abstract class ScenarioGrain : Grain, IScenarioGrain
    {
        protected Guid OrganisationId { get; set; }

        private readonly ICollection<StreamSubscriptionHandle<object>> subscriptions;
        private readonly Type[] streamTypes;
        protected IList<string> issuedCommands { get; }
        public List<ScenarioEventHandler> eventHandlers;

        protected ScenarioGrain(params Type[] streamTypes)
        {
            this.subscriptions = new Collection<StreamSubscriptionHandle<object>>();
            this.streamTypes = streamTypes;
            this.eventHandlers = new List<ScenarioEventHandler>();
        }

        public override async Task OnActivateAsync()
        {
            OrganisationId = this.GetPrimaryKey();

            foreach (var type in streamTypes)
            {
                var stream = this.GetStreamProvider("RedisProvider").GetStream<object>(OrganisationId, type.Name);
                subscriptions.Add(await stream.SubscribeAsync(new ScenarioEventObserver(this)));
            }
        }

        public override async Task OnDeactivateAsync()
        {
            foreach (var sub in subscriptions)
            {
                await sub.UnsubscribeAsync();
            }
        }

        public abstract Task ExecuteAsync();


        public async Task RegisterSubscription<TStream>(Guid streamId)
        {
            var stream = GetStreamProvider("RedisProvider").GetStream<object>(streamId, typeof(TStream).Name);
            Console.WriteLine($"Registering stream for {streamId}-{typeof(TStream).Name}");
            subscriptions.Add(await stream.SubscribeAsync(new ScenarioEventObserver(this)));
        }

        public void On<T>(Func<T, Task> action)
        {
            eventHandlers.Add(new ScenarioEventHandler(typeof(T), async (@event) =>
            {
                await action((T)@event);
            }, (ev) => true));
        }

        public void On<T>(Func<T, Task> action, Func<T, bool> shouldExecute)
        {
            eventHandlers.Add(new ScenarioEventHandler(typeof(T), async (@event) => await action((T)@event), (@event) => shouldExecute((T)@event)));
        }

        public async Task On(object @event)
        {
            var type = @event.GetType();
            var handlers = eventHandlers.Where(p => p.Item1.IsAssignableFrom(type));
            foreach (var handler in handlers.ToList())
            {
                if (handler.Item3(@event))
                {
                    await handler.Item2(@event);
                }
            }
        }

        protected string GetCommandId()
        {
            var commandId = Guid.NewGuid().ToString();
            issuedCommands.Add(commandId);
            return commandId;
        }
    }
}
