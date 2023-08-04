using DutchTreat.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace DutchTreat
{
    public class Program
    {
        public static void Main(string[] args)
        {

            IHost host = BuildWebHost(args);

            //if (args.Length == 1 && args[0].ToLower() == "/seed")
            //   RunSeeding(host);
            //else
            host.Run();
        }

        private static void RunSeeding(IHost host)
        {
            IServiceScopeFactory scopeFactory = host.Services.GetService<IServiceScopeFactory>();

            using (IServiceScope scope = scopeFactory.CreateScope())
            {
                DutchSeeder seeder = scope.ServiceProvider.GetService<DutchSeeder>();
                seeder.Seed();
            }
        }

        public static IHost BuildWebHost(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(AddConfiguration)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).Build();

        private static void AddConfiguration(HostBuilderContext context, IConfigurationBuilder builder)
        {
            builder.Sources.Clear();

            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json", false, true)
                .AddEnvironmentVariables();
        }
    }
}
