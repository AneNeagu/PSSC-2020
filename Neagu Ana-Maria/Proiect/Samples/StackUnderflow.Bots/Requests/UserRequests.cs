using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using GraphQL.Types;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using StackUnderflow.Domain.Schema.Voting.VoteUpOp;

namespace StackUnderflow.Bots.Requests
{
    public static class UserRequests
    {
        //public static async Task<GraphQLResponse<dynamic>> CreateQuestion(this GraphQLHttpClient client, CreateQuestionCmd createQuestionCmd)
        //{
        //    const string query = 
        //        @"mutation createQuestion($cmd: CreateQuestionCmd!) {
        //            createQuestion(cmd: $cmd) {
        //            successful
        //            }
        //        }";

        //    var request = new GraphQLRequest(query, new Dictionary<string, object>()
        //    {
        //        {"cmd", createQuestionCmd}
        //    });

        //    return await client.SendMutationAsync<dynamic>(request);
        //}

        public static async Task<GraphQLResponse<dynamic>> LoadQuestions(this GraphQLHttpClient client)
        {
            const string query =
                @"query {
                          questions {
                            questions{
      	                        questionId
      	                        title
      	                        lastUpdatedText
                            }
                          }
                    }";
            
            var request = new GraphQLRequest(query, new Dictionary<string, object>());
            return await client.SendQueryAsync<dynamic>(request);

        }

        //public static async Task<GraphQLResponse<dynamic>> CreateAnswer(this GraphQLHttpClient client, CreateAnswerCmd cmd)
        //{
        //    const string query =
        //        @"mutation createAnswer($cmd: CreateAnswerCmd!){
        //            createAnswer(cmd: $cmd){
        //                successful
        //                answerId
        //                failureText
        //            }
        //        }";

        //    var request = new GraphQLRequest(query, new Dictionary<string, object>()
        //    {
        //        {"cmd", cmd}
        //    });

        //    return await client.SendMutationAsync<dynamic>(request);
        //}

        public static async Task<GraphQLResponse<dynamic>> VoteUp(this GraphQLHttpClient client, VoteUpCmd cmd)
        {
            const string query =
                @"mutation voteUp($cmd: VoteUpCmd!){
                    voteUp(cmd: $cmd){
                        successful
                        failureText
                    }
                }";

            var request = new GraphQLRequest(query, new Dictionary<string, object>()
            {
                {"cmd", cmd}
            });

            return await client.SendMutationAsync<dynamic>(request);
        }
    }
}
