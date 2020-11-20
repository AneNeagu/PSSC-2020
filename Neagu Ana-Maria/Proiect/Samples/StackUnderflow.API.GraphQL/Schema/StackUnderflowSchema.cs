using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orleans;

namespace StackUnderflow.API.GraphQL.Schema
{
    public class StackUnderflowSchema : global::GraphQL.Types.Schema
    {
        public StackUnderflowSchema(IServiceProvider serviceProvider, IClusterClient clusterClient) : base(serviceProvider)
        {
            Query = new StackUnderflowQuery();
            Mutation = new StackUnderflowMutation(clusterClient);
        }
    }
}
