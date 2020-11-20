using System;
using GraphQL.Client.Http;

namespace StackUnderflow.Bots.Requests
{
    public static class Extensions
    {
        public static GraphQLHttpClient WithOrgHeaders(this GraphQLHttpClient client, Guid organisationId, int tenantId)
        {
            client.HttpClient.DefaultRequestHeaders.Add("OrganisationId", organisationId.ToString());
            client.HttpClient.DefaultRequestHeaders.Add("TenantId", tenantId.ToString());
            return client;
        }

        public static GraphQLHttpClient WithUserId(this GraphQLHttpClient client, Guid userId)
        {
            client.HttpClient.DefaultRequestHeaders.Add("UserId", userId.ToString());
            return client;
        }
    }
}
