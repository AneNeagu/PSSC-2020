using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Access.Primitives.IO;
using LanguageExt;
using StackUnderflow.Domain.Schema.Backoffice.CreateTenantOp;
using Access.Primitives.Extensions.ObjectExtensions;
using Access.Primitives.IO.Attributes;
using Access.Primitives.IO.Mocking;
using StackUnderflow.Domain.Core.Contexts;
using StackUnderflow.EF.Models;
using static StackUnderflow.Domain.Schema.Backoffice.CreateTenantOp.CreateTenantResult;
using StackUnderflow.Domain.Schema.Backoffice;

namespace StackUnderflow.Backoffice.Adapters.CreateTenant
{
    public partial class CreateTenantAdapter : Adapter<CreateTenantCmd, ICreateTenantResult, BackofficeWriteContext, BackofficeDependencies>
    {
        private readonly IExecutionContext _ex;

        public CreateTenantAdapter(IExecutionContext ex)
        {
            _ex = ex;
        }

        public override async Task<ICreateTenantResult> Work(CreateTenantCmd command, BackofficeWriteContext state, BackofficeDependencies dependencies)
        {
            var workflow = from valid in command.TryValidate()
                           let t = AddTenantIfMissing(state, CreateTenantFromCommand(command))
                           select t;


            var result = await workflow.Match(
                Succ: r => r,
                Fail: ex => new InvalidRequest(ex.ToString()));

            return result;
        }
       
        public ICreateTenantResult AddTenantIfMissing(BackofficeWriteContext state, Tenant tenant)
        {
            if (state.Tenants.Any(p => p.Name.Equals(tenant.Name)))
                return new TenantNotCreated();

            if (state.Tenants.All(p => p.TenantId != tenant.TenantId))
                state.Tenants.Add(tenant);
            return new TenantCreated(tenant, tenant.TenantUser.Single().User);
        }

        private Tenant CreateTenantFromCommand(CreateTenantCmd cmd)
        {
            var tenant = new Tenant()
            {
                Description = cmd.Description,
                Name = cmd.TenantName,
                OrganisationId = cmd.OrganisationId,
            };
            tenant.TenantUser.Add(new TenantUser()
            {
                User = new User()
                {
                    UserId = cmd.UserId,
                    Name = cmd.AdminName,
                    Email = cmd.AdminEmail,
                    DisplayName = cmd.AdminName,
                    WorkspaceId = cmd.UserId
                }
            });
            return tenant;
        }

        public override Task PostConditions(CreateTenantCmd op, CreateTenantResult.ICreateTenantResult result, BackofficeWriteContext state)
        {
            return Task.CompletedTask;
        }
    }
}
