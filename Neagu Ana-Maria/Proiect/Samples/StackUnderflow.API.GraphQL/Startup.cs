using System;
using System.Linq;
using GraphQL.Server;
using GraphQL.Server.Ui.Altair;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackUnderflow.API.GraphQL.Handlers;
using StackUnderflow.API.GraphQL.Schema;
using StackUnderflow.API.GraphQL.Middleware;
using System.Security.Claims;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using StackUnderflow.API.GraphQL.UserAndTenant;
using Microsoft.AspNetCore.Authentication;

namespace FakeSO.API.GraphQL
{
    public class Startup
    {

        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment env)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy
                        .WithOrigins("http://localhost:3000")
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services
                .AddGraphQL()
                .AddSystemTextJson(deserializerSettings => { }, serializerSettings => { })
                .AddWebSockets()
                .AddDataLoader()
                .AddGraphTypes(typeof(StackUnderflowSchema));

            services
                .AddAuthenticationHandlers(Configuration);

            services.AddSingleton<IClaimsTransformation, ClaimsTransformer>();

            services.AddScoped<UserContext>(ctx =>
            {
                var returnValue = new UserContext();
                var contextAccessor = ctx.GetService<IHttpContextAccessor>();
                if (contextAccessor != null)
                {
                    var httpContext = contextAccessor.HttpContext;
                    string userIdString = (httpContext.User?.Identity as ClaimsIdentity)?.Claims.FirstOrDefault(ctx => ctx.Type == "acid")?.Value;
                    var userId = Guid.Parse(userIdString);
                    returnValue.UserId = userId;
                }
                return returnValue;
            });

            services.AddSingleton<StackUnderflowSchema>();

            services.AddHttpContextAccessor();

            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSetupRequestContext();
            app.Use(async (context, next) =>
            {
                context.Request.EnableBuffering();
                await next.Invoke();
            });

            app.UseCors();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
        
            app.UseOpenTracingForOrleans();
            app.Map("/graphql", GraphQLAuthenticationHandler.Handle);

            app.UseWebSockets();

            app.UseGraphQLWebSockets<StackUnderflowSchema>();
            app.UseGraphQL<StackUnderflowSchema>("/graphql");
            app.UseGraphQLPlayground();
            app.UseGraphQLAltair(new GraphQLAltairOptions());

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
