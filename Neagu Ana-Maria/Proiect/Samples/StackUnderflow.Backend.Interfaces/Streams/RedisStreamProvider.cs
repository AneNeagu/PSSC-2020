using System;
using System.Collections.Generic;
using System.Text;
using Access.Primitives.Orleans.Streaming;

namespace StackUnderflow.Backend.Abstractions.Streams
{
    public class RedisStreamProvider : StreamProviderReference
    {
        public RedisStreamProvider() : base("RedisProvider")
        {
        }
    }
}
