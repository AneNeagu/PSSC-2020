using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrainImplementation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace FakeSO.API.Rest
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
                .UseOrleans(siloBuilder =>
                {
                    siloBuilder.UseLocalhostClustering()
                    .Configure<ClusterOptions>(options =>
                     {
                         options.ClusterId = "dev";
                         options.ServiceId = "OrleansBasics";
                     })
                    .ConfigureApplicationParts(
                        parts => parts.AddApplicationPart(typeof(EmailSenderGrain).Assembly)
                                .WithReferences());
                });
    }
}
