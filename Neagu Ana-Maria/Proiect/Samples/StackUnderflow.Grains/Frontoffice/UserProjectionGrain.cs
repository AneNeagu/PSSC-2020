using Access.Primitives.EFCore;
using Access.Primitives.Orleans;
using Aqua.Dynamic;
using Microsoft.EntityFrameworkCore;
using Orleans;
using Orleans.Streams;
using StackUnderflow.Backend.Abstractions.FrontOffice;
using StackUnderflow.Domain.Core.Contexts.Questions;
using StackUnderflow.Domain.Schema.Models;
using StackUnderflow.EF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Access.Primitives.IO;
using LanguageExt;
using StackUnderflow.EF;

namespace StackUnderflow.Backend.Grains.Frontoffice
{
    public class UserProjectionGrain : ProjectionGrain<UserReadContext>, IUserProjectionGrain
    {
        private readonly IInterpreterAsync _interpreter;
        private readonly Port<UserDbContext> _dbContextPort;

        private UserDbContext _dbContext;
        private UserReadContext _questionsProjection;
        private UserGrainId _userGrainId;
        private readonly ICollection<StreamSubscriptionHandle<object>> _subscriptions;

        public UserProjectionGrain(IInterpreterAsync interpreter, Port<UserDbContext> dbContextPort)
        {
            _interpreter = interpreter;
            _dbContextPort = dbContextPort;
            _subscriptions = new Collection<StreamSubscriptionHandle<object>>();
        }

        public override async Task OnActivateAsync()
        {
            _userGrainId = UserGrainId.FromString(this.GetPrimaryKeyString());
            _dbContext = await _interpreter.Interpret(_dbContextPort, Unit.Default);
            _dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            await GetData();
        }

        private async Task GetData() {
            _questionsProjection = await _dbContext.LoadAsync("base.UserProjection", new { UserId = _userGrainId.UserId },
                    async reader =>
                    {
                        var questionSummaries = await reader.ReadAsync<QuestionSummary>();
                        return new UserReadContext(questionSummaries);
                    });
        }

        protected override Func<Type, IQueryable> QueryableResourceProvider => type =>
        {
            switch (type.Name)
            {
                case nameof(UserReadContext):
                    return new UserReadContext[] { _questionsProjection }.AsQueryable();
                default:
                    throw new NotSupportedException($"{this.GetType().Name} doesn't know how to handle {type.Name}");
            }
        };
    }
}
