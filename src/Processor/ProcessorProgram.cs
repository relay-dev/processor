using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Processor.Internal;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Processor
{
    public class ProcessorProgram<TStartup> where TStartup : IProcessorStartup
    {
        protected static async Task RunAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            // Build the configuration
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true)
                .AddJsonFile($"appsettings.{EnvironmentName}.json", true)
                .AddJsonFile("C:\\Azure\\appsettings.KeyVault.json", true, true)
                .AddJsonFile("appsettings.Local.json", true, true)
                .AddJsonFile("local.settings.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            // Initialize a ServiceCollection
            var services = new ServiceCollection()
                .AddSingleton<IConfiguration>(config)
                .AddLogging(builder =>
                {
                    builder.AddConfiguration(config.GetSection("Logging"));
                    builder.AddConsole();
                    builder.AddDebug();
                });

            // Add Processors
            AddProcessorTypes(services);

            // Create the Startup
            IProcessorStartup startup = (TStartup)Activator.CreateInstance(typeof(TStartup), new object[] { config });

            if (startup == null)
            {
                throw new InvalidOperationException($"Could not create an instance of startup class of type '{typeof(TStartup).FullName}'");
            }

            // Configure services
            startup.ConfigureServices(services);

            // Build the IServiceProvider
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // Create the program
            var application = new ProcessorProgramRunner(serviceProvider);

            try
            {
                // Run the program
                await application.RunAsync(cancellationToken);
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine("Encountered unhandled exception:");
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }

        private static IServiceCollection AddProcessorTypes(IServiceCollection services)
        {
            var types = new AssemblyScanner().GetTypesWithAttribute<ProcessorAttribute>();

            foreach (Type type in types)
            {
                services.AddTransient(type);
            }

            return services;
        }

        private static string EnvironmentName => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    }
}
