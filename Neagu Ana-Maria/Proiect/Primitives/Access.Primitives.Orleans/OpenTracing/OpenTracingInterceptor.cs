using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OpenTracing;
using OpenTracing.Util;
using Orleans;
using Orleans.Runtime;

namespace Access.Primitives.Orleans.OpenTracing
{
    public class OpenTracingInterceptor : IIncomingGrainCallFilter
    {
        public async Task Invoke(IIncomingGrainCallContext context)
        {
            var spanContext = RequestContext.Get("SpanContext") as ISpanContext;
            if (spanContext == null || !typeof(IGrain).IsAssignableFrom(context.Grain.GetType()))
            {
                await context.Invoke();
                return;
            }


            using (GlobalTracer.Instance.BuildSpan(context.ImplementationMethod.Name).AsChildOf(spanContext)
                .WithTag("grain-id", context.Grain.GetPrimaryKeyString())
                .WithTag("grain-name", context.Grain.GetType().Name)
                .StartActive(true))
            {

                await context.Invoke();
            }
        }
    }
}
