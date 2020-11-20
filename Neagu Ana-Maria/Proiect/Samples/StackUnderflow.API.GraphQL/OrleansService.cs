using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Orleans.Hosting;
using StackUnderflow.Backend.Silo;
using System.Threading;
using System.Threading.Tasks;

namespace FakeSO.API.GraphQL
{
    internal class OrleansService : IHostedService
    {
        private static readonly IConfigurationRoot Configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json", false)
           .Build();

        private ISiloHost _silo;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _silo = new SiloHostBuilder()
                .OrleansSelfhost(Configuration)
                .Build();

            await _silo.StartAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _silo.StopAsync();
        }
    }
}