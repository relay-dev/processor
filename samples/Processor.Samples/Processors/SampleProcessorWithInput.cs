using System;
using System.Threading;
using System.Threading.Tasks;

namespace Processor.Samples.Processors
{
    [Processor]
    public class SampleProcessorWithInput : IProcessor<object>
    {
        public async Task ProcessAsync(object input, CancellationToken cancellationToken)
        {
            Console.WriteLine($"{GetType().Name} is processing...");
        }
    }
}
