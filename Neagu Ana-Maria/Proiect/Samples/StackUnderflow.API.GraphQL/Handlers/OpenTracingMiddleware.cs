using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OpenTracing;
using Orleans.Runtime;

namespace StackUnderflow.API.GraphQL.Handlers
{
    public class OpenTracingMiddleware
    {
        private readonly RequestDelegate _next;

        public OpenTracingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext ctx)
        {
            var tracer = ctx.RequestServices.GetService<ITracer>();
            RequestContext.Set("SpanContext", tracer.ActiveSpan.Context);
            await _next(ctx);
        }
    }

    public static class OpenTracingMiddlewareExtensions
    {
        public static IApplicationBuilder UseOpenTracingForOrleans(this IApplicationBuilder app)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            return app.UseMiddleware<OpenTracingMiddleware>();
        }
    }
}