using Microsoft.Extensions.Configuration;

namespace Processor.Samples.Services.Impl
{
    public class SampleService : ISampleService
    {
        private readonly IConfiguration _configuration;

        public SampleService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConfigValue(string key)
        {
            return _configuration[key];
        }
    }
}
