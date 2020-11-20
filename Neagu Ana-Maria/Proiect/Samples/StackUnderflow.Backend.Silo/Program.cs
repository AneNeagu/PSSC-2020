using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Access.Primitives.EFCore.DSL;
using Access.Primitives.IO;
using Access.Primitives.IO.Mocking;
using StackUnderflow.Backend.Grains;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Hosting;
using Orleans.Runtime;
using StackUnderflow.EF.Models;
using Microsoft.Extensions.Logging;
using Access.Primitives.IO.Extensions;
using Access.Primitives.Orleans.Streaming.Adapters;
using Access.Primitives.Orleans.OpenTracing;
using Access.Primitives.Orleans.Streaming;
using OpenTracing;
using OpenTracing.Util;
using StackUnderflow.Backend.Abstractions.Streams;
using StackUnderflow.Backend.Grains.Frontoffice;
using StackUnderflow.Backend.Grains.StreamGrain;
using StackUnderflow.Backoffice.Adapters.CreateTenant;
using StackUnderflow.EF;

namespace StackUnderflow.Backend.Silo
{
    public static class HostExtensions
    {
        public static ISiloHostBuilder OrleansSelfhost(this ISiloHostBuilder hostBuilder, IConfigurationRoot configuration)
        {
            return hostBuilder
                    .UseLocalhostClustering()
                    .ConfigureServices(services =>
                    {
                        services.AddOperations(typeof(CreateTenantAdapter).Assembly);
                        services.AddOperations(typeof(PublishAdapter).Assembly);

                        services.AddTransient<ITracer>(sp => GlobalTracer.Instance);

                        services.AddSingleton<IInterpreterAsync>(sp => new LiveInterpreterAsync(sp));
                        services.AddTransient<IExecutionContext, LiveExecutionContext>();
                        services.AddStreamProviderRefs(typeof(RedisStreamProvider).Assembly);
                        services.AddDbContext<StackUnderflowContext>(options =>
                        {
                            options.UseSqlServer(
                                configuration.GetConnectionString("StackUnderflow"),
                                options =>
                                {
                                    options.EnableRetryOnFailure(maxRetryCount: 10,
                                        maxRetryDelay: TimeSpan.FromSeconds(30), null);
                                });
                        });
                        services.AddDbContext<UserDbContext>(options =>
                        {
                            options.UseSqlServer(
                                configuration.GetConnectionString("StackUnderflow"),
                                options =>
                                {
                                    options.EnableRetryOnFailure(maxRetryCount: 10,
                                        maxRetryDelay: TimeSpan.FromSeconds(30), null);
                                });
                        });

                        services.AddTransient<Port<StackUnderflowContext>>(StackUnderflowContext.Factory);
                        services.AddTransient<Port<UserDbContext>>(sp =>
                        {
                            var grainActivationContext = sp.GetService<IGrainActivationContext>();
                            var userGrainId = UserGrainId.FromString(grainActivationContext.GrainIdentity.PrimaryKeyString);
                            return UserDbContext.DbContextFactory(sp, userGrainId.UserId.ToString());
                        });
                    })
                    .AddIncomingGrainCallFilter<OpenTracingInterceptor>()
                    .ConfigureApplicationParts(parts =>
                    {
                        parts.AddApplicationPart(typeof(BackofficeGrain).Assembly).WithReferences()
                            .WithCodeGeneration();
                    })
                    .AddRedisStreams("RedisProvider",
                        c => c.ConfigureRedis(options => options.ConnectionString = configuration.GetConnectionString("RedisConnectionString")))
                    .AddMemoryGrainStorage("PubSubStore")
                    .ConfigureLogging(builder =>
                    {
                        builder.AddConsole();
                    })
                    .AddStartupTask<StartupTask>();
        }
    }

    class Program
    {
        private static readonly IConfigurationRoot Configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json", false)
           .Build();

        static void Main(string[] args)
        {
            var host = new SiloHostBuilder()
                .OrleansSelfhost(Configuration)
                .Build();

            host.StartAsync().Wait();
            Console.ReadLine();
            Console.WriteLine("Hello World!");
        }
    }

    internal class StartupTask : IStartupTask
    {
        private readonly IClusterClient _clusterClient;

        public StartupTask(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }
        public async Task Execute(CancellationToken cancellationToken)
        {
        }
    }
}
