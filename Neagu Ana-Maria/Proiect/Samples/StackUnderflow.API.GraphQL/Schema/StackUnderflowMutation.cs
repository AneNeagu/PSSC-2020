using System;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using Orleans;
using Orleans.Runtime;
using StackUnderflow.API.GraphQL.Handlers;
using StackUnderflow.API.GraphQL.Schema.Commands;
using StackUnderflow.API.GraphQL.Schema.Responses;
using StackUnderflow.Domain.Schema.Voting.VoteUpOp;
using StackUnderflow.Backend.Interfaces;
using StackUnderflow.Domain.Schema.Backoffice.CreateTenantOp;
using StackUnderflow.Backend.Abstractions;
using StackUnderflow.Domain.Schema.Backoffice.InviteUserOp;
using StackUnderflow.Backend.Abstractions.FrontOffice;
using StackUnderflow.Backend.Abstractions.Fontoffice;
using StackUnderflow.Domain.Schema.Voting.VoteDownOp;
using StackUnderflow.Domain.Schema.Voting.CancelVoteOp;
using StackUnderflow.API.GraphQL.Middleware;
using StackUnderflow.Backend.Grains.Frontoffice;

namespace StackUnderflow.API.GraphQL.Schema
{
    public class StackUnderflowMutation : ObjectGraphType
    {
        private readonly IClusterClient _clusterClient;

        public StackUnderflowMutation(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
            FieldAsync<CreateTenantAndAdminResponseG>("createTenant", arguments: new QueryArguments()
            {
                new QueryArgument<NonNullGraphType<CreateTenantCmdG>> {Name = "cmd"}
            }, resolve: CreateTenant);
            FieldAsync<InviteUserResponseG>("inviteUser", arguments: new QueryArguments()
            {
                new QueryArgument<NonNullGraphType<InviteUserCmdG>> {Name = "cmd"}
            }, resolve: InviteUser);
            //FieldAsync<CreateQuestionResponseG>("createQuestion", arguments: new QueryArguments()
            //{
            //    new QueryArgument<NonNullGraphType<CreateQuestionCmdG>> {Name = "cmd"}
            //}, resolve: CreateQuestion);
            //FieldAsync<CreateAnswerResponseG>("createAnswer", arguments: new QueryArguments()
            //{
            //    new QueryArgument<NonNullGraphType<CreateAnswerCmdG>> {Name = "cmd"}
            //}, resolve: CreateAnswer);
            //FieldAsync<VoteResponseG>("voteUp", arguments: new QueryArguments()
            //{
            //    new QueryArgument<NonNullGraphType<VoteUpCmdG>> {Name = "cmd"}
            //}, resolve: VoteUp);
            //FieldAsync<VoteResponseG>("voteDown", arguments: new QueryArguments()
            //{
            //    new QueryArgument<NonNullGraphType<VoteDownCmdG>> {Name = "cmd"}
            //}, resolve: VoteDown);
            //FieldAsync<VoteResponseG>("cancelVote", arguments: new QueryArguments()
            //{
            //    new QueryArgument<NonNullGraphType<CancelVoteCmdG>> {Name = "cmd"}
            //}, resolve: CancelVote);
        }

        private async Task<object> CreateTenant(IResolveFieldContext<object> ctx)
        {
            var backofficeGrain = _clusterClient.GetGrain<IBackofficeGrain>(Guid.Empty, "backoffice");
            var cmd = ctx.GetArgument<CreateTenantCmd>("cmd");
            var response = await backofficeGrain.CreateTenantAndAdmin(cmd.OrganisationId, cmd.TenantName, cmd.Description,
                cmd.AdminEmail, cmd.AdminName, cmd.UserId);
            return response;
        }

        private async Task<object> InviteUser(IResolveFieldContext<object> ctx)
        {
            var backofficeGrain = _clusterClient.GetGrain<IBackofficeGrain>(Guid.Empty, "backoffice");
            var cmd = ctx.GetArgument<InviteUserCmd>("cmd");
            var response = await backofficeGrain.InviteUser(cmd.OrganisationId, cmd.UserId, cmd.UserEmail, cmd.UserName);
            return response;
        }

        //private async Task<object> CreateQuestion(IResolveFieldContext<object> ctx)
        //{
        //    var userId = (ctx.UserContext["Result"] as GraphQLUserContext).UserId;
        //    var cmd = ctx.GetArgument<CreateQuestionCmd>("cmd");
        //    var organisationId = (ctx.UserContext["Result"] as GraphQLUserContext).OrganisationId;
        //    var userGrain = _clusterClient.GetGrain<IUserGrain>(new UserGrainId(organisationId, userId, "user").ToString());
        //    var response = await userGrain.CreateQuestion(cmd.Title, cmd.Body);
        //    return response;
        //}

        //private async Task<object> CreateAnswer(IResolveFieldContext<object> ctx)
        //{
        //    var userId = (ctx.UserContext["Result"] as GraphQLUserContext).UserId;
        //    var cmd = ctx.GetArgument<CreateAnswerCmd>("cmd");
        //    var organisationId = (ctx.UserContext["Result"] as GraphQLUserContext).OrganisationId;
        //    var questionGrain = _clusterClient.GetGrain<IQuestionGrain>(new QuestionGrainId(organisationId, cmd.QuestionId, "question").ToString());
        //    var response = await questionGrain.CreateAnswer(userId, cmd.QuestionId, cmd.Answer);
        //    return response;
        //}

        //private async Task<object> VoteUp(IResolveFieldContext<object> ctx)
        //{
        //    var userId = (ctx.UserContext["Result"] as GraphQLUserContext).UserId;
        //    var organisationId = (ctx.UserContext["Result"] as GraphQLUserContext).OrganisationId;
        //    var cmd = ctx.GetArgument<VoteUpCmd>("cmd");
        //    var questionGrain = _clusterClient.GetGrain<IQuestionGrain>(new QuestionGrainId(organisationId, cmd.QuestionId, "question").ToString()); 
        //    var response = await questionGrain.VoteUp(userId);
        //    return response;
        //}

        //private async Task<object> VoteDown(IResolveFieldContext<object> ctx)
        //{
        //    var userId = (ctx.UserContext["Result"] as GraphQLUserContext).UserId;
        //    var organisationId = (ctx.UserContext["Result"] as GraphQLUserContext).OrganisationId;
        //    var cmd = ctx.GetArgument<VoteUpCmd>("cmd");
        //    var questionGrain = _clusterClient.GetGrain<IQuestionGrain>(new QuestionGrainId(organisationId, cmd.QuestionId, "question").ToString());
        //    var response = await questionGrain.VoteDown(userId);
        //    return response;
        //}

        //private async Task<object> CancelVote(IResolveFieldContext<object> ctx)
        //{
        //    var userId = (ctx.UserContext["Result"] as GraphQLUserContext).UserId;
        //    var organisationId = (ctx.UserContext["Result"] as GraphQLUserContext).OrganisationId;
        //    var cmd = ctx.GetArgument<VoteUpCmd>("cmd");
        //    var questionGrain = _clusterClient.GetGrain<IQuestionGrain>(new QuestionGrainId(organisationId, cmd.QuestionId, "question").ToString());
        //    var response = await questionGrain.CancelVote(userId);
        //    return response;
        //}

    }
}