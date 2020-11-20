using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Access.Primitives.IO.Mocking;
using Microsoft.Extensions.DependencyInjection;

namespace Access.Primitives.IO.Extensions.xUnit
{
    public class AdapterTest
    {
        private readonly Assembly[] _adapterAssemblies;

        public AdapterTest(params Assembly[] adapterAssemblies)
        {
            _adapterAssemblies = adapterAssemblies;
        }

        public IServiceProvider CreateServiceProvider(MockContext exec, params Assembly[] adapterAssemblies)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<MockInterpreterAsync>();
            serviceCollection.AddSingleton<LiveInterpreterAsync>();
            foreach (var assembly in adapterAssemblies)
            {
                serviceCollection.AddOperations(assembly);
            }
            serviceCollection.AddScoped<IExecutionContext>(sp => exec.CreateScope());
            return serviceCollection.BuildServiceProvider();
        }

        public async Task<A> TestExpr<T, D, A>(T state, D dependencies, Port<A> expr, params object[] paths)
        {
            var idempotency = paths.OfType<Idempotency>().SingleOrDefault();
            var mockContext = MockContext.GetInstance(paths);
            var sp = CreateServiceProvider(mockContext, _adapterAssemblies);
            var interpreter = new MockInterpreterAsync(sp);

            A result = default;
            switch (idempotency)
            {
                case Idempotency.RunOnce:
                     result = await interpreter.Interpret(expr, state, dependencies);
                     break;
                case Idempotency.RunTwice:
                    result = await interpreter.Interpret(expr, state, dependencies);
                    result = await interpreter.Interpret(expr, state, dependencies);
                    break;
            }
            return result;
        }

    }
}
