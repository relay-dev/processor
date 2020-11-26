using Processor.Samples.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Processor.Samples.Processors
{
    [Processor]
    public class SampleProcessorWithDependency : IProcessor
    {
        private readonly ISampleService _service;

        public SampleProcessorWithDependency(ISampleService service)
        {
            _service = service;
        }
        
        public async Task ProcessAsync(CancellationToken cancellationToken)
        {
            string configValue = _service.GetConfigValue("AppSettingTest");

            Console.WriteLine($"ConfigValue: {configValue}");
        }
    }
}
