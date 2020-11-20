using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OpenTracing;
using OpenTracing.Util;
using Orleans;
using Orleans.Runtime;

namespace StackUnderflow.Bots
{
    public class StreamInterceptor : IIncomingGrainCallFilter
    {
        public async Task Invoke(IIncomingGrainCallContext context)
        {
            if (!context.InterfaceMethod.Name.Equals("DeliverBatch"))
            {
                await context.Invoke();
                return;
            }

            var spanContext = RequestContext.Get("SpanContext") as ISpanContext;
            if (spanContext != null)
            {
                using (GlobalTracer.Instance.BuildSpan(context.ImplementationMethod.Name).AsChildOf(spanContext)
                    .WithTag("grain-id", context.Grain.GetPrimaryKeyString())
                    .WithTag("grain-name", context.Grain.GetType().Name)
                    .StartActive(true))
                {

                    await context.Invoke();
                }
            }
            else
            {
                await context.Invoke();
            }

        }
    }
}
