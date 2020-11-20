using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Access.Primitives.Extensions.Cloning;
using Access.Primitives.IO;
using Access.Primitives.Orleans.Streaming.Adapters;
using LanguageExt;
using static PortExt;

namespace Access.Primitives.Orleans.Streaming
{
    public static class Streaming
    {
        public static Port<PublishResult> Publish(string provider, Guid partition, string topic, object @event) =>
            NewPort<PublishCmd, PublishResult>(new PublishCmd(provider, partition, topic, new[] { @event }));

        public static Port<PublishResult> PublishAll(string provider, Guid partition, string topic,
            params object[] events)
            => NewPort<PublishCmd, PublishResult>(new PublishCmd(provider, partition, topic, events));
    }


}
