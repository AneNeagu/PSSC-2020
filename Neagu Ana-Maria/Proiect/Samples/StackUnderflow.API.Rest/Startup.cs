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
using StackUnderflow.Backoffice.Adapters.CreateTenant;
using StackUnderflow.EF.Models;

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

            services.AddDbContext<StackUnderflowContext>(builder =>
            {
                builder.UseSqlServer(Configuration.GetConnectionString("StackUnderflow"));
            });

            services.AddControllers();
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
    }
}
