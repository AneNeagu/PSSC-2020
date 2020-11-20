using System;
using System.Linq;
using System.Threading.Tasks;
using Access.Primitives.EFCore;
using Access.Primitives.Extensions.Cloning;
using Access.Primitives.Extensions.ObjectExtensions;
using Access.Primitives.IO;
using Access.Primitives.Orleans.Streaming;
using Access.Primitives.Orleans.Streaming.Adapters;
using LanguageExt;
using StackUnderflow.Backend.Interfaces;
using Microsoft.EntityFrameworkCore;
using Orleans;
using Orleans.Streams;
using StackUnderflow.Backend.Interfaces.Responses;
using StackUnderflow.Domain.Core;
using StackUnderflow.Domain.Core.Contexts;
using StackUnderflow.Domain.Schema.Backoffice.CreateTenantOp;
using StackUnderflow.EF.Models;
using static StackUnderflow.Domain.Schema.Backoffice.CreateTenantOp.CreateTenantResult;
using StackUnderflow.Backend.Abstractions.Responses;
using StackUnderflow.Backend.Abstractions.Streams;
using StackUnderflow.Domain.Schema.Backoffice.InviteUserOp;

namespace StackUnderflow.Backend.Grains
{
    public class BackofficeGrain : Grain, IBackofficeGrain
    {
        private readonly IInterpreterAsync _interpreter;
        private readonly Port<StackUnderflowContext> _dbContextFactory;
        private StackUnderflowContext _dbContext;
        private readonly RedisStreamProvider _providerRef;
        private BackofficeWriteContext _ctx;

        public BackofficeGrain(IInterpreterAsync interpreter, Port<StackUnderflowContext> dbContextFactory, RedisStreamProvider providerRef)
        {
            _interpreter = interpreter;
            _dbContextFactory = dbContextFactory;
            _providerRef = providerRef;
        }

        public override async Task OnActivateAsync()
        {
            _dbContext = await _interpreter.Interpret(_dbContextFactory, Unit.Default);

            _ctx = await _dbContext.LoadAsync("base.BackofficeGrain", new object(), async reader =>
            {
                var tenants = await reader.ReadAsync<Tenant>();
                var tenantUsers = await reader.ReadAsync<TenantUser>();
                var users = await reader.ReadAsync<User>();

                _dbContext.AttachRange(tenants);
                _dbContext.AttachRange(tenantUsers);
                _dbContext.AttachRange(users);
                return new BackofficeWriteContext(
                    new EFList<Tenant>(_dbContext.Tenant),
                    new EFList<TenantUser>(_dbContext.TenantUser),
                    new EFList<User>(_dbContext.User));
            });
        }

        public async Task<CreateTenantAndAdminResponse> CreateTenantAndAdmin(Guid organisationId, string tenantName, string description, string adminEmail, string adminName, Guid userId)
        {
            //define
            var expr = from createResult in BackofficeDomain.CreateTenant(organisationId, tenantName, description, adminEmail, adminName, userId)
                       let adminUser = createResult.SafeCast<TenantCreated>().Select(p => p.User)
                       from inviteResult in BackofficeDomain.InviteTenantAdmin(adminUser)
                       from setPermissionsResult in BackofficeDomain.SetPermissions(adminUser.Select(p=>p.UserId), 1)
                       from publishResult in Publish(typeof(object).Name, createResult)
                       select createResult;

            //execute
            var r = await _interpreter.Interpret(expr, this._ctx);
            await _dbContext.SaveChangesAsync();

            //adapt
            return r.Match(
                created => new CreateTenantAndAdminResponse(true, created.Tenant.TenantId, created.User.DisplayName),
                notCreated => new CreateTenantAndAdminResponse(false, 0, string.Empty),
                invalidRequest => new CreateTenantAndAdminResponse(false, 0, string.Empty));
        }

        public async Task<InviteUserResponse> InviteUser(Guid organisationId, Guid userId, string email, string userName)
        {
            var expr = from createResult in BackofficeDomain.InviteUser(organisationId, userId, email, userName)
                       from publishResult in Publish(typeof(object).Name, createResult)
                       select createResult;

            var r = await _interpreter.Interpret(expr, this._ctx);

            await _dbContext.SaveChangesAsync();

            return r.Match(
                created => new InviteUserResponse(true, created.User.UserId, created.User.DisplayName),
                notCreated => new InviteUserResponse(false, Guid.Empty, string.Empty),
                invalidRequest => new InviteUserResponse(false, Guid.Empty, string.Empty));
        }


        #region partial application

        private Port<PublishResult> Publish(string topic, object @event) =>
            Streaming.Publish(_providerRef.ProviderName, this.GetPrimaryKey(out string _), topic, @event);

        private Port<PublishResult> Publish(string topic, params object[] events) =>
            Streaming.PublishAll(_providerRef.ProviderName, this.GetPrimaryKey(out string _), topic, events);

        #endregion
    }
}
