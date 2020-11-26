using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Processor.Samples.Services;
using Processor.Samples.Services.Impl;

namespace Processor.Samples
{
    public class Startup : IStartup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ISampleService, SampleService>();
        }
    }
}
