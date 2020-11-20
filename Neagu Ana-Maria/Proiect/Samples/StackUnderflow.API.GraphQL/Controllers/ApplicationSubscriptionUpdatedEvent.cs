using System;

namespace StackUnderflow.API.GraphQL.Controllers
{
    internal class ApplicationSubscriptionUpdatedEvent
    {

        public ApplicationSubscription ApplicationSubscription { get; set; }
    }

    public class ApplicationSubscription
    {
        public bool Active { get; set; }
        public Organisation Organisation { get; set; }
    }

    public class Organisation
    {
        public Guid Id{ get; set; }
        public string Name { get; set; }
    }
}