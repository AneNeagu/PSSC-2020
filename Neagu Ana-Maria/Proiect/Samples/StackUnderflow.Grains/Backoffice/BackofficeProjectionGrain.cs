using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Access.Primitives.EFCore;
using Access.Primitives.Orleans;
using Microsoft.EntityFrameworkCore;
using StackUnderflow.Backend.Abstractions.Backoffice;
using StackUnderflow.Domain.Core.Contexts;
using StackUnderflow.EF.Models;

namespace StackUnderflow.Backend.Grains.Backoffice
{
    public class BackofficeProjectionGrain : ProjectionGrain<BackofficeReadContext>, IBackofficeProjection
    {
        private readonly StackUnderflowContext _dbContext;
        private BackofficeReadContext _backofficeProjection;

        public BackofficeProjectionGrain(StackUnderflowContext dbContext)
        {
            _dbContext = dbContext;
            _dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public override async Task OnActivateAsync()
        {
            _backofficeProjection = await _dbContext.LoadAsync("dbo.BackofficeProjection", new object(),
                async reader =>
                {
                    var tenants = await reader.ReadAsync<Tenant>();
                    var tenantUsers = await reader.ReadAsync<TenantUser>();
                    var users = await reader.ReadAsync<User>();

                    _dbContext.AttachRange(tenants);
                    _dbContext.AttachRange(tenantUsers);
                    _dbContext.AttachRange(users);

                    return new BackofficeReadContext(_dbContext.Tenant.Local.ToList(), _dbContext.User.Local.ToList());
                });
        }

        protected override Func<Type, IQueryable> QueryableResourceProvider => type =>
        {
            switch (type.Name)
            {
                case nameof(BackofficeReadContext):
                    return new BackofficeReadContext[] {_backofficeProjection}.AsQueryable();
                default:
                    throw new NotSupportedException($"{this.GetType().Name} doesn't know how to handle {type.Name}");
            }
        };
    }
}
