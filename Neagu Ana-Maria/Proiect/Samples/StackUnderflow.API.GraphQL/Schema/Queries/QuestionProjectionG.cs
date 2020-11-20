using Access.Primitives.Orleans;
using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Remote.Linq;
using StackUnderflow.API.GraphQL.Middleware;
using StackUnderflow.API.GraphQL.Schema.Commands;
using StackUnderflow.Backend.Abstractions.Fontoffice;
using StackUnderflow.Backend.Abstractions.FrontOffice;
using StackUnderflow.Backend.Grains.Frontoffice;
using StackUnderflow.Domain.Core.Contexts.Questions;
using StackUnderflow.Domain.Schema.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackUnderflow.API.GraphQL.Schema.Queries
{
    public class QuestionProjectionG : ObjectGraphType
    {
        private readonly IClusterClient _clusterClient;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public QuestionProjectionG(IClusterClient clusterClient, IServiceScopeFactory serviceScopeFactory)
        {
            _clusterClient = clusterClient;
            _serviceScopeFactory = serviceScopeFactory;
            FieldAsync<ListGraphType<QuestionG>>("question", arguments: new QueryArguments()
            {
                new QueryArgument<NonNullGraphType<GetQuestionCmdG>> {Name = "cmd"}
            }, resolve: ResolveQuestion);
        }

        private async Task<object> ResolveQuestion(IResolveFieldContext<object> arg)
        {
            var cmd = arg.GetArgument<GetQuestionCmd>("cmd");
            var userId = (arg.UserContext["Result"] as GraphQLUserContext).UserId;
            var organisationId = (arg.UserContext["Result"] as GraphQLUserContext).OrganisationId;
            var grain = _clusterClient.GetGrain<IQuestionProjectionGrain>(new QuestionGrainId(organisationId, cmd.QuestionId, "user").ToString());
            var remote = new RemoteRepository<IQuestionProjectionGrain, QuestionReadContext>(grain);
            var questions = await (from r in remote.Root
                                   from u in r.Questions
                                   select u).ToListAsync();

            var questionGrain = _clusterClient.GetGrain<IQuestionGrain>(new QuestionGrainId(organisationId, cmd.QuestionId, "question").ToString());
            //await questionGrain.MarkAsViewed(userId);

            return questions;

        }

    }

}
