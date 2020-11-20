using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Primitives.Orleans.Streaming
{
    
    public class StreamProviderReference
    {
        private readonly string _providerName;

        public StreamProviderReference(string providerName)
        {
            _providerName = providerName;
        }

        public string ProviderName => _providerName;
    }
}
