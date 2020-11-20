using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Orleans.Runtime;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StackUnderflow.API.GraphQL.Handlers
{
    public static class RequestContextConst
    {
        public const string OrganisationId = "OrganisationId";
        public const string TenantId = "TenantId";
        public const string UserId = "UserId";
    }

    public class SetupRequestContextMiddleware
    {
        private readonly RequestDelegate _next;

        public SetupRequestContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("OrganisationId", out var organisationIds) &&
                organisationIds.Any() &&
                Guid.TryParse(organisationIds.First(), out var organisationId))
            {
                RequestContext.Set(RequestContextConst.OrganisationId, organisationId);
            }

            if (context.Request.Headers.TryGetValue("TenantId", out var tenantIds) &&
                tenantIds.Any() &&
                int.TryParse(tenantIds.First(), out var tenantId))
            {
                RequestContext.Set(RequestContextConst.TenantId, tenantId);
            }

            if (context.Request.Headers.TryGetValue("UserId", out var userIds) && userIds.Any() &&
                Guid.TryParse(userIds.First(), out var userId))
            {
                RequestContext.Set(RequestContextConst.UserId, userId);
            }

            await _next(context);
        }
    }

    public static class SetupRequestContextMiddlewareExtensions
    {
        public static IApplicationBuilder UseSetupRequestContext(this IApplicationBuilder app)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            return app.UseMiddleware<SetupRequestContextMiddleware>();
        }
    }
}
