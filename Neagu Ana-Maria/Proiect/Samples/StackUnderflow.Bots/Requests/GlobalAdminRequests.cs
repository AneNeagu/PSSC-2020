using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using NUnit.Framework;
using StackUnderflow.Domain.Schema.Backoffice.CreateTenantOp;

namespace StackUnderflow.Bots.Requests
{
    public static class GlobalAdminRequests
    {
        public static async Task<GraphQLResponse<dynamic>> CreateTenantAndAdmin(this GraphQLHttpClient client, CreateTenantCmd createTenantCmd)
        {
            const string query =
                @"mutation createTenant($cmd: CreateTenantCmd!) {
                      createTenant(cmd: $cmd) {
                        successful 
                        tenantId
                        adminName
                      }
                }";

            var request = new GraphQLRequest(query, new Dictionary<string, object>()
            {
                {"cmd", createTenantCmd}
            });

            var result = await client.SendMutationAsync<dynamic>(request);
            Assert.IsNull(result.Errors);

            return result;
        }
    }
}
