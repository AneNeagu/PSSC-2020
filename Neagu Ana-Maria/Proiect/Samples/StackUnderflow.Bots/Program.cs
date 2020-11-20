using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FakeSO.API.GraphQL;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Runtime;
using Orleans.Streams;
using Orleans.Serialization;
using StackUnderflow.Backend.Grains;
using StackUnderflow.Backend.Interfaces;
using StackUnderflow.Bots.Scenarios;
using StackUnderflow.EF.Models;
using Microsoft.Extensions.Logging;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using Jaeger;
using Jaeger.Samplers;
using Microsoft.ApplicationInsights.Extensibility;
using OpenTracing;
using OpenTracing.Util;
using Petabridge.Tracing.ApplicationInsights;

namespace StackUnderflow.Bots
{
    class Program
    {
        static void Main(string[] args)
        {
            new HostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<ITracer>(sp =>
                    {
                        string serviceName = Assembly.GetEntryAssembly().GetName().Name;
                        ILoggerFactory loggerFactory = sp.GetRequiredService<ILoggerFactory>();
                        ISampler sampler = new ConstSampler(true);

                        var telemetryConfig =
                            new TelemetryConfiguration("eb9eab65-f8b2-4af5-9c4c-1fc7912b8ac3");
                        var builder = telemetryConfig.TelemetryProcessorChainBuilder;
                        builder.Use((next) => new AppinsightsOpenTracingInterceptor(next));
                        builder.Build();

                        //ApplicationInsightsTracer tracer =
                        //    new ApplicationInsightsTracer(telemetryConfig);

                        ITracer tracer = new Tracer.Builder(serviceName)
                            .WithLoggerFactory(loggerFactory)
                            .WithSampler(sampler)
                            .Build();

                        GlobalTracer.Register(tracer);
                        return tracer;
                    });
                    services.AddOpenTracing();
                }).Build().Start();

            var host = new SiloHostBuilder()
                .UseLocalhostClustering()
                .ConfigureEndpoints(IPAddress.Parse("127.0.0.1"), 11111, 30000)
                .ConfigureApplicationParts(parts =>
                {
                    parts.AddApplicationPart(typeof(EndToEndScenarioGrain).Assembly).WithReferences()
                        .WithCodeGeneration();
                    parts.AddApplicationPart(typeof(BackofficeGrain).Assembly).WithReferences().WithCodeGeneration();
                })
                .AddRedisStreams("RedisProvider", // Add the Redis stream provider
                    c => c.ConfigureRedis(options => options.ConnectionString = "localhost"))
                .AddMemoryGrainStorage("PubSubStore")
                .AddIncomingGrainCallFilter<StreamInterceptor>()
                .ConfigureServices(services =>
                {
                    services.AddHttpClient("ref", client =>
                    {
                        client.BaseAddress = new Uri("http://localhost:5000/graphql");
                    });
                    services.AddSingleton<Func<GraphQLHttpClient>>((sp) => () =>
                        new GraphQLHttpClient(new GraphQLHttpClientOptions(), new SystemTextJsonSerializer(), 
                            sp.GetRequiredService<IHttpClientFactory>().CreateClient("ref")));

                  
                })
                /*.ConfigureLogging(builder => {
                    builder.AddConsole();
                })*/
                .AddStartupTask<StartupTask>()
                .Build();

            host.StartAsync().Wait();
            Console.ReadLine();
        }
    }

    internal class StartupTask : IStartupTask
    {
        private readonly IClusterClient _botsClusterClient;

        public StartupTask(IClusterClient botsClusterClient)
        {
            _botsClusterClient = botsClusterClient;
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            var e2e = _botsClusterClient.GetGrain<IEndToEndScenarioGrain>(Guid.NewGuid());
            await e2e.ExecuteAsync();
        }
    }
}
