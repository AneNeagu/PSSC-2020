using GraphQL.Client.Http;
using StackUnderflow.Bots.Requests;
using StackUnderflow.Domain.Schema.Backoffice.CreateTenantOp;
using StackUnderflow.Domain.Schema.Backoffice.InviteTenantAdminOp;
using StackUnderflow.EF.Models;
using System;
using System.Threading.Tasks;
using OpenTracing;
using OpenTracing.Util;
using Orleans.Runtime;

namespace StackUnderflow.Bots.Scenarios
{
    public interface IEndToEndScenarioGrain : IScenarioGrain
    {
    }

    public class EndToEndScenarioGrain : ScenarioGrain, IEndToEndScenarioGrain
    {
        private readonly Func<GraphQLHttpClient> _clientFactory;

        private Tenant Tenant { get; set; }
        private User User { get; set; }

        public EndToEndScenarioGrain(Func<GraphQLHttpClient> clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();
            await RegisterSubscription<object>(Guid.Empty);
            await RegisterSubscription<object>(OrganisationId);
            //await RegisterSubscription<CreateQuestionResult.ICreateQuestionResult>(OrganisationId);
        }

        public override async Task ExecuteAsync()
        {
            using (GlobalTracer.Instance
                .BuildSpan(nameof(EndToEndScenarioGrain))
                .WithTag("IntegrationTest", "EndToEndScenario")
                .StartActive())
            {

                On<CreateTenantResult.ICreateTenantResult>(OnTenantCreated);
                On<InviteTenantAdminResult.IInviteTenantAdminResult>(OnAdminInvited);
                //On<CreateQuestionResult.ICreateQuestionResult>(OnQuestionCreated);

                using var client = _clientFactory();
                var result = await client.CreateTenantAndAdmin(new CreateTenantCmd(OrganisationId,
                    Guid.NewGuid().ToString(),
                    Guid.NewGuid().ToString(),
                    "cosmin.tiru@gmail.com",
                    Guid.NewGuid().ToString(), Guid.NewGuid()));
                Console.WriteLine(result.Data);
            }
        }

        private Task OnAdminInvited(InviteTenantAdminResult.IInviteTenantAdminResult arg)
        {
            arg.Match(invited =>
            {
                Console.WriteLine("Invited");
                arg.ConsoleLogAsIdentedJson();

                return invited;
            }, notInvited =>
            {
                Console.WriteLine("Not invited");
                return notInvited;
            }, invalidRequest => invalidRequest);

            return Task.CompletedTask;
        }

        private async Task OnTenantCreated(CreateTenantResult.ICreateTenantResult arg)
        {
            await arg.MatchAsync(async created =>
                {
                    var spanContext = RequestContext.Get("SpanContext") as ISpanContext;
                    Console.WriteLine("Tenant created");
                    arg.ConsoleLogAsIdentedJson();

                    Tenant = created.Tenant;
                    User = created.User;

                    // Post question in new tenant by new user
                    using var client = _clientFactory().WithOrgHeaders(
                        created.Tenant.OrganisationId.Value,
                        created.Tenant.TenantId)
                        .WithUserId(User.UserId);

                    //var result = await UserRequests.CreateQuestion(
                    //    client,
                    //    new CreateQuestionCmd(
                    //        title: $"Question-{Guid.NewGuid()}",
                    //        body: "End-to-End-test"));
                    //Console.WriteLine(result.Data);

                    /**using var questionClient = _clientFactory().WithOrgHeaders(
                        created.Tenant.OrganisationId.Value,
                        created.Tenant.TenantId);
                    var questionsResult = await UserRequests.LoadQuestions(questionClient);
                    Console.WriteLine(questionsResult.Data);*/

                    return created;
                }, async notCreated =>
                {
                    // Should we be able to get the existing tenant here?

                    Console.WriteLine("Tenant not created");
                    return await Task.FromResult(notCreated);
                },
                async invalidRequest =>
                {
                    Console.WriteLine("Invalid request");
                    return await Task.FromResult(invalidRequest);
                });
        }

        //private async Task OnQuestionCreated(CreateQuestionResult.ICreateQuestionResult arg)
        //{
        //    await arg.MatchAsync(
        //        async created =>
        //        {
        //            Console.WriteLine("Question created");
        //            arg.ConsoleLogAsIdentedJson();

        //            using var client = _clientFactory().WithOrgHeaders(
        //                Tenant.OrganisationId.Value,
        //                Tenant.TenantId)
        //                .WithUserId(User.UserId);
        //            var questions = await client.LoadQuestions();
        //            questions.ConsoleLogAsIdentedJson();

        //            using var simpleClient = _clientFactory()
        //                .WithOrgHeaders(Tenant.OrganisationId.Value, Tenant.TenantId)
        //                .WithUserId(User.UserId);
        //            var result = await UserRequests.CreateAnswer(
        //                simpleClient,
        //                new CreateAnswerCmd(
        //                    questionId: created.Post.PostId,
        //                    answer: "Test-Answer"));
        //            Console.WriteLine(result.Data);

        //            // Vote up, down
        //            var voteUpResult = await UserRequests.VoteUp(
        //                simpleClient,
        //                new VoteUpCmd(questionId: created.Post.PostId));
        //            Console.WriteLine(voteUpResult.Data);

        //            return created;
        //        },
        //        notCreated =>
        //        {
        //            Console.WriteLine("Question not created");
        //            return Task.FromResult<CreateQuestionResult.ICreateQuestionResult>(notCreated);
        //        },
        //        invalidRequest =>
        //        {
        //            Console.WriteLine("Error on creating question");
        //            return Task.FromResult<CreateQuestionResult.ICreateQuestionResult>(invalidRequest);
        //        });
        //}
    }
}
