using GraphQL;
using GraphQL.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using StackUnderflow.API.GraphQL.Schema;
using StackUnderflow.API.GraphQL.UserAndTenant;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Orleans.Runtime;
using StackUnderflow.API.GraphQL.Handlers;

namespace StackUnderflow.API.GraphQL.Middleware
{
    public class IdentityAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public IdentityAuthenticationMiddleware(RequestDelegate next)
        {
            if (next == null)
                throw new ArgumentNullException("next");
            _next = next;

        }

        public async Task Invoke(HttpContext context)
        {
            bool isIntrospectiveQuery = await RequestIsIntrospectiveQuery(context);

            if (context.User.Identity.IsAuthenticated | isIntrospectiveQuery)
            {
                await _next(context); // either an authenticated request or the graphql playground makes its schema request
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new { error = "unauthorised" }));
                return;
            }
        }
        static async Task<bool> RequestIsIntrospectiveQuery(HttpContext context)
        {
            bool isIntrospectiveQuery = true;

            // todo: only do this if in dev mode to support the playground ??
            context.Request.Body.Seek(0, SeekOrigin.Begin); // todo: do I need this ?
            string bodyContent = await new StreamReader(context.Request.Body).ReadToEndAsync();
            context.Request.Body.Seek(0, SeekOrigin.Begin); // todo: do I need this ?
            isIntrospectiveQuery = bodyContent.Contains("IntrospectionQuery");
            return isIntrospectiveQuery;
        }
    }


    public class GraphQLAuthenticationHandler
    {
        public static void Handle(IApplicationBuilder app)
        {
            var settings = new GraphQLSettings
            {
                BuildUserContext = ctx =>
                {
                    Guid userId = Guid.Empty;
                    string userIdString = ctx.User.Claims.FirstOrDefault(c => c.Type == "acid")?.Value;
                    if (userIdString != null)
                    {
                        userId = Guid.Parse(userIdString);
                    }
                    else
                    {
                        var userIdValue = RequestContext.Get(RequestContextConst.UserId)?.ToString();
                        if(!string.IsNullOrWhiteSpace(userIdValue))
                            userId = Guid.Parse(RequestContext.Get(RequestContextConst.UserId)?.ToString());
                    }

                    Guid organisationId = Guid.Empty;
                    string organisationIdString = ctx.User.Claims.FirstOrDefault(c => c.Type == "organisationId")?.Value;
                    if (organisationIdString != null)
                    {
                        organisationId = Guid.Parse(organisationIdString);
                    }
                    else
                    {
                        var organisationIdValue = RequestContext.Get(RequestContextConst.OrganisationId)?.ToString();
                        if (!string.IsNullOrWhiteSpace(organisationIdValue))
                            organisationId = Guid.Parse(RequestContext.Get(RequestContextConst.OrganisationId)?.ToString());
                    }

                    var userContext = new GraphQLUserContext
                    {
                        UserId = userId,
                        OrganisationId = organisationId
                    };

                    return Task.FromResult(userContext);
                }
            };
#if !DEBUG
            app.UseMiddleware<IdentityAuthenticationMiddleware>();
#else
            app.UseSetupRequestContext();
#endif
            app.UseMiddleware<GraphQLMiddleware>(settings);
            app.UseGraphQL<StackUnderflowSchema>(new PathString(""));
        }
    }

}
