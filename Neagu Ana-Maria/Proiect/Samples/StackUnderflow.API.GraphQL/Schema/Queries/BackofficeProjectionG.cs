using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Access.Primitives.Orleans;
using GraphQL.Types;
using Orleans;
using Remote.Linq;
using StackUnderflow.Backend.Abstractions.Backoffice;
using StackUnderflow.Domain.Core.Contexts;

namespace StackUnderflow.API.GraphQL.Schema.Queries
{
    public class BackofficeProjectionG : ObjectGraphType
    {
        private readonly IClusterClient _clusterClient;

        public BackofficeProjectionG(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
            FieldAsync<ListGraphType<TenantG>>("tenants", resolve: ResolveTenants);
            FieldAsync<ListGraphType<UserG>>("users", resolve: ResolveUsers);
        }

        private async Task<object> ResolveUsers(IResolveFieldContext<object> arg)
        {
            var grain = _clusterClient.GetGrain<IBackofficeProjection>(Guid.Empty, "backoffice-projection");
            var remote = new RemoteRepository<IBackofficeProjection, BackofficeReadContext>(grain);
            var users = await (from r in remote.Root
                               from u in r.Users
                               select u).EvalAsync();
            return users;
        }

        private async Task<object> ResolveTenants(IResolveFieldContext<object> arg)
        {
            var grain = _clusterClient.GetGrain<IBackofficeProjection>(Guid.Empty, "backoffice-projection");
            var remote = new RemoteRepository<IBackofficeProjection, BackofficeReadContext>(grain);
            var tenants = await (from r in remote.Root
                from t in r.Tenants
                select t).EvalAsync();

            return tenants;
        }
    }
}
