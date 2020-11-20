using Access.Primitives.EFCore;
using Access.Primitives.Orleans;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Orleans;
using Orleans.Streams;
using StackUnderflow.Backend.Abstractions.FrontOffice;
using StackUnderflow.Domain.Core.Contexts.Questions;
using StackUnderflow.EF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackUnderflow.Backend.Grains.Frontoffice
{

    public class QuestionGrainId
    {
        public Guid OrganisationId { get; }
        public int QuestionId { get; }
        public string Suffix { get; }

        public QuestionGrainId(Guid organisationId, int questionId, string suffix)
        {
            OrganisationId = organisationId;
            QuestionId = questionId;
            Suffix = suffix;
        }

        public static QuestionGrainId FromString(string id)
        {
            var split = id.Split('/');
            return new QuestionGrainId(Guid.Parse(split[0]), Convert.ToInt32(split[1]), split[2]);
        }

        public override string ToString()
        {
            return $"{OrganisationId}/{QuestionId}/{Suffix}";
        }
    }

    public class QuestionProjectionGrain : ProjectionGrain<QuestionReadContext>, IQuestionProjectionGrain
    {

        private readonly StackUnderflowContext _dbContext;
        private QuestionReadContext _questionProjection;
        private QuestionGrainId _questionGrainId;
        private readonly ICollection<StreamSubscriptionHandle<object>> _subscriptions;

        public QuestionProjectionGrain(StackUnderflowContext dbContext)
        {
            _dbContext = dbContext;
            _dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _subscriptions = new Collection<StreamSubscriptionHandle<object>>();

        }

        public override async Task OnActivateAsync()
        {
            _questionGrainId = QuestionGrainId.FromString(this.GetPrimaryKeyString());

            await GetData();
        }

        private async Task GetData()
        {

            var questionDic = new Dictionary<int, Post>();
            var answerDic = new Dictionary<int, Post>();
            var voteDic = new Dictionary<string, Vote>();

            var result = await _dbContext.Database.GetDbConnection().QueryAsync<Post, Post, Vote, Post>("base.QuestionProjection", (question, answer, vote) =>
            {

                Post q;
                if(!questionDic.TryGetValue(question.PostId, out q))
                {
                    q = question;
                    questionDic.Add(q.PostId, q);
                }


                if (answer != null)
                {
                    Post a;
                    if (!answerDic.TryGetValue(answer.PostId, out a))
                    {
                        answerDic.Add(answer.PostId, answer);
                        q.InversePostNavigation.Add(answer);
                    }
                }

                if (vote != null)
                {
                    Vote v;
                    if (!voteDic.TryGetValue($"{vote.QuestionId}/{vote.UserId}", out v))
                    {
                        voteDic.Add($"{vote.QuestionId}/{vote.UserId}", vote);
                        q.Vote.Add(vote);
                    }
                }

                return q;
            },
            param: new { QuestionId = _questionGrainId.QuestionId },
            commandType: System.Data.CommandType.StoredProcedure,
            splitOn: "PostId,PostId,QuestionId");

            _questionProjection = new QuestionReadContext(result.Distinct());
        }

        protected override Func<Type, IQueryable> QueryableResourceProvider => type =>
        {
            switch (type.Name)
            {
                case nameof(QuestionReadContext):
                    return new QuestionReadContext[] { _questionProjection }.AsQueryable();
                default:
                    throw new NotSupportedException($"{this.GetType().Name} doesn't know how to handle {type.Name}");
            }
        };
    }
}
