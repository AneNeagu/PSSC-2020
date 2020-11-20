using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Access.Primitives.IO.Extensions.xUnit;
using StackUnderflow.Backoffice.Adapters;
using StackUnderflow.Backoffice.Adapters.CreateTenant;
using StackUnderflow.Domain.Core;
using StackUnderflow.Domain.Core.Contexts;
using StackUnderflow.Domain.Schema.Backoffice.CreateTenantOp;
using Xunit;
using static PortExt;
using static StackUnderflow.Backoffice.Adapters.CreateTenant.CreateTenantAdapter;

namespace StackUnderflow.Adapters.Tests
{
    public class CreateTenantAdapterProofs: AdapterTest
    {
        public CreateTenantAdapterProofs() : base(typeof(CreateTenantAdapter).Assembly) { }

        [Theory]
        //[CarthesianProductOf(typeof(CreateTenantCmdInput), typeof(BackofficeWriteContextInput), typeof(Idempotency))]
        [CarthesianProductOf(typeof(CreateTenantCmdInput), typeof(BackofficeWriteContextInput))]
        public async Task CreateTenantAdapter(params object[] path)
        {
            var input = new CreateTenantCmdInputGen().Get(path.OfType<CreateTenantCmdInput>().Single());
            var ctx = new BackofficeWriteContextGen().Get(path.OfType<BackofficeWriteContextInput>().Single());

            var expr = from t in BackofficeDomain.CreateTenant(input.OrganisationId, input.TenantName,
                            input.Description, input.AdminEmail, input.AdminName, input.UserId)
                       select t;

            var result =
                await TestExpr(ctx, expr, path);

        }
    }
}
