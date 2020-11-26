using System;
using System.Threading;
using System.Threading.Tasks;

namespace Processor.Samples.Processors
{
    [Processor]
    public class SampleProcessorWithInputOutput : IProcessor<object, object>
    {
        public async Task<object> ProcessAsync(object input, CancellationToken cancellationToken)
        {
            Console.WriteLine($"{GetType().Name} is processing...");

            return new object();
        }
    }
}
