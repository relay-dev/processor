using System;
using System.Threading;
using System.Threading.Tasks;

namespace Processor.Samples.Processors
{
    [Processor(Category = "New Category", Name = "ProcessorWithNameAndCategory")]
    public class SampleProcessorWithNameAndCategory : IProcessor
    {
        public async Task ProcessAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"{GetType().Name} is processing...");
        }
    }
}
