using Access.Primitives.EFCore;
using Access.Primitives.Orleans;
using Microsoft.EntityFrameworkCore;
using Orleans;
using Orleans.Streams;
using StackUnderflow.Backend.Abstractions;
using StackUnderflow.Domain.Core.Contexts;
using StackUnderflow.EF.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StackUnderflow.Backend.Grains
{
    public class QuestionsProjectionGrain : ProjectionGrain<QuestionsReadContext>, IQuestionsProjection  //IAsyncObserver<ICreateQuestionResult>
    {
        private readonly StackUnderflowContext _dbContext;
        private QuestionsReadContext _questionsProjection;

        private Guid _organisationId;
        public int _tenantId;

        public QuestionsProjectionGrain(StackUnderflowContext dbContext)
        {
            _dbContext = dbContext;
            _dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public override async Task OnActivateAsync()
        {
            GrainKey.TryParse(this.GetPrimaryKeyString(), out _organisationId, out _tenantId, out var name);

            _questionsProjection = await _dbContext.LoadAsync("dbo.QuestionsProjection",
                new { TenantId = _tenantId },
                async reader =>
                {
                    var posts = await reader.ReadAsync<Post>();

                    _dbContext.AttachRange(posts);

                    return new QuestionsReadContext(_dbContext.Post.Local.ToList());
                });

        }

        //public Task OnNextAsync(ICreateQuestionResult item, StreamSequenceToken token = null)
        //{
        //    DeactivateOnIdle();
        //    return Task.CompletedTask;
        //}

        public Task OnCompletedAsync()
        {
            throw new NotImplementedException();
        }

        public Task OnErrorAsync(Exception ex)
        {
            throw new NotImplementedException();
        }

        protected override Func<Type, IQueryable> QueryableResourceProvider => type =>
        {
            switch (type.Name)
            {
                case nameof(QuestionsReadContext):
                    return new QuestionsReadContext[] { _questionsProjection }.AsQueryable();
                default:
                    throw new NotSupportedException($"{this.GetType().Name} doesn't know how to handle {type.Name}");
            }
        };
    }
}
