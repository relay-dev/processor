using System;
using System.Threading;
using System.Threading.Tasks;

namespace Processor.Samples.Processors
{
    [Processor]
    public class SampleProcessor : IProcessor
    {
        public async Task ProcessAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"{GetType().Name} is processing...");
        }
    }
}
