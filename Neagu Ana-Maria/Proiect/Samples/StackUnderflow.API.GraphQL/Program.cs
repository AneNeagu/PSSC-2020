using System.Reflection;
using Jaeger;
using Jaeger.Samplers;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Util;
using Orleans;
using Orleans.Hosting;
using Petabridge.Tracing.ApplicationInsights;

namespace FakeSO.API.GraphQL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices(s =>
                {
                    s.AddSingleton<ITracer>(sp =>
                    {
                        string serviceName = Assembly.GetEntryAssembly().GetName().Name;
                        ILoggerFactory loggerFactory = sp.GetRequiredService<ILoggerFactory>();
                        ISampler sampler = new ConstSampler(true);

                        var telemetryConfig =
                            new TelemetryConfiguration("eb9eab65-f8b2-4af5-9c4c-1fc7912b8ac3");
                        var builder = telemetryConfig.TelemetryProcessorChainBuilder;
                        builder.Use((next) => new AppinsightsOpenTracingInterceptor(next));
                        builder.Build();

                        ApplicationInsightsTracer tracer =
                            new ApplicationInsightsTracer(telemetryConfig);

                        //ITracer tracer = new Tracer.Builder(serviceName)
                        //    .WithLoggerFactory(loggerFactory)
                        //    .WithSampler(sampler)
                        //    .Build();

                        GlobalTracer.Register(tracer);
                        return tracer;
                    });
                    s.AddOpenTracing();

                    s.AddHostedService<OrleansService>();
                    s.AddSingleton(sp => GetSiloClusterClient());
                });

        private static IClusterClient GetSiloClusterClient()
        {
            var client = new ClientBuilder()
                .UseLocalhostClustering()
                .ConfigureApplicationParts(parts =>
                {
                    parts.AddApplicationPart(typeof(StackUnderflow.Backend.Interfaces.IBackofficeGrain).Assembly)
                    .WithReferences().WithCodeGeneration();
                })
                .AddRedisStreams("RedisProvider", c => c.ConfigureRedis(options => options.ConnectionString = "localhost"))
                .Build();
            client.Connect().Wait();
            return client;
        }
    }

    public class AppinsightsOpenTracingInterceptor : ITelemetryProcessor
    {
        private readonly ITelemetryProcessor _next;

        public AppinsightsOpenTracingInterceptor(ITelemetryProcessor next)
        {
            _next = next;
        }

        public void Process(ITelemetry item)
        {
            item.Context.Cloud.RoleName = Assembly.GetExecutingAssembly().GetName().Name;
            this._next.Process(item);
        }
    }
}
