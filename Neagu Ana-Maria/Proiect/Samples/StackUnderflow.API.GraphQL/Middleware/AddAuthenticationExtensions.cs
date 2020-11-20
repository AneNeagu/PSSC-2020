using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackUnderflow.API.GraphQL.Middleware
{
    /// <summary>
    /// Extensions for registering Authentication services
    /// </summary>
    public static class AddAuthenticationExtensions
    {
        /// <summary>
        /// Adds the services which are being used for Authentication
        /// </summary>
        /// <param name="services"></param>
        /// <param name="Configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddAuthenticationHandlers(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            .AddIdentityServerAuthentication(options =>
            {
                options.Authority = Configuration.GetSection("AppSettings")["AccessIdentityAuthority"];
                options.NameClaimType = "email";
            });

            return services;
        }
    }
}
