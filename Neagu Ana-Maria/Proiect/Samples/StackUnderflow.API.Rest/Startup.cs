using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Access.Primitives.IO;
using Access.Primitives.IO.Extensions;
using Access.Primitives.IO.Mocking;
using Jaeger;
using Jaeger.Samplers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Util;
using Orleans;
using StackUnderflow.Backoffice.Adapters.CreateTenant;
using StackUnderflow.EF;
using StackUnderflow.EF.Models;
using Orleans.Hosting;

namespace FakeSO.API.Rest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOperations(typeof(CreateTenantAdapter).Assembly);
            services.AddSingleton<IExecutionContext, LiveExecutionContext>();
            services.AddTransient<IInterpreterAsync>(sp => new LiveInterpreterAsync(sp));

            services.AddDbContext<DatabaseContext>(builder =>
            {
                builder.UseSqlServer(Configuration.GetConnectionString("StackUnderflow"));
            });

            services.AddControllers();
            services.AddSingleton(sp => GetSiloClusterClient());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        private static IClusterClient GetSiloClusterClient()
        {
            //ApplicationPartManagerCodeGenExtensions

            var client = new ClientBuilder()
                .UseLocalhostClustering()
                .ConfigureApplicationParts(parts =>
                {
                    parts.AddApplicationPart(typeof(GrainInterfaces.IHello).Assembly)
                    .WithReferences()
                    ;
                })
                .AddSimpleMessageStreamProvider("SMSProvider", options => { options.FireAndForgetDelivery = true; })
                //.AddRedisStreams("RedisProvider", c => c.ConfigureRedis(options => options.ConnectionString = "localhost"))
                .Build();
            client.Connect().Wait();
            return client;
        }



    }
}
