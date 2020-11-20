using Access.Primitives.Orleans;
using GraphQL.Types;
using Orleans;
using StackUnderflow.Backend.Abstractions.FrontOffice;
using StackUnderflow.Domain.Core.Contexts.Questions;
using System;
using System.Threading.Tasks;
using System.Linq;
using Remote.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using StackUnderflow.API.GraphQL.UserAndTenant;
using Microsoft.AspNetCore.Http;
using Orleans.Runtime;
using StackUnderflow.API.GraphQL.Handlers;
using StackUnderflow.API.GraphQL.Middleware;
using StackUnderflow.Backend.Grains.Frontoffice;

namespace StackUnderflow.API.GraphQL.Schema.Queries
{

    public class QuestionsProjectionG : ObjectGraphType
    {
        private readonly IClusterClient _clusterClient;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public QuestionsProjectionG(IClusterClient clusterClient, IServiceScopeFactory serviceScopeFactory)
        {
            _clusterClient = clusterClient;
            _serviceScopeFactory = serviceScopeFactory;
            FieldAsync<ListGraphType<QuestionSummaryG>>("questions", resolve: ResolveQuestions);
        }

        private async Task<object> ResolveQuestions(IResolveFieldContext<object> arg)
        {
            var userId = (arg.UserContext["Result"] as GraphQLUserContext).UserId;
            var organisationId = (arg.UserContext["Result"] as GraphQLUserContext).OrganisationId;
            var grain = _clusterClient.GetGrain<IUserProjectionGrain>(new UserGrainId(organisationId, userId, "user-projection").ToString());
            var remote = new RemoteRepository<IUserProjectionGrain, UserReadContext>(grain);
            var questions = await (from r in remote.Root
                                   from u in r.Questions
                                   select u).ToListAsync();
            return questions;

        }

    }
}
