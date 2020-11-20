using Access.Primitives.IO.Extensions.xUnit;
using LanguageExt;
using StackUnderflow.Adapters.InviteAdmin;
using StackUnderflow.Domain.Core;
using StackUnderflow.Domain.Core.Contexts;
using StackUnderflow.Domain.Schema.Backoffice.InviteTenantAdminOp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StackUnderflow.Adapters.Tests
{
    public class InviteTenantAdminAdapterProofs : AdapterTest
    {
        public InviteTenantAdminAdapterProofs() : base(typeof(InviteTenantAdminAdapter).Assembly)
        {
        }

        [Theory]
        [CarthesianProductOf(typeof(InviteTenantAdminCmdInput), typeof(SendGridEffect), typeof(BackofficeWriteContextInput))]
        public async Task InviteTenantAdminAdapter_Tests(params object[] path)
        {
            var input = new InviteTenantAdminInputGen().Get(path.OfType<InviteTenantAdminCmdInput>().Single());
            var ctx = new BackofficeWriteContextGen().Get(path.OfType<BackofficeWriteContextInput>().Single());

            var expr = from t in BackofficeDomain.InviteTenantAdmin(input.AdminUser)
                       select t;

            var result = await TestExpr(ctx, expr, path);
        }
    }
}
