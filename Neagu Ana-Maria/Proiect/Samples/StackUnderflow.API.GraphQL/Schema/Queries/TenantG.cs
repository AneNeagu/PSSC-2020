using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;
using StackUnderflow.EF.Models;

namespace StackUnderflow.API.GraphQL.Schema.Queries
{
    public class TenantG : AutoRegisteringObjectGraphType<Tenant>
    {
        public TenantG()
        {
            Field<ListGraphType<UserG>>("users", resolve: ResolveUsers);
        }

        private object ResolveUsers(IResolveFieldContext<Tenant> ctx)
        {
            return ctx.Source.TenantUser.Select(p => p.User);
        }
    }
}
