using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Processor
{
    public abstract class ProcessorBase : IProcessor
    {
        private IServiceProvider _serviceProvider;

        public virtual IProcessor Initialize(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            return this;
        }

        protected async Task RunAsync<TProcessor>(CancellationToken cancellationToken) where TProcessor : IProcessor
        {
            IProcessor processor = _serviceProvider.GetRequiredService<TProcessor>();

            await processor.ProcessAsync(cancellationToken);
        }

        protected async Task RunAsync<TProcessor, TProcessorInput>(TProcessorInput input, CancellationToken cancellationToken) where TProcessor : IProcessor<TProcessorInput>
        {
            IProcessor<TProcessorInput> processor = _serviceProvider.GetRequiredService<TProcessor>();

            await processor.ProcessAsync(input, cancellationToken);
        }

        protected async Task<TProcessorOutput> RunAsync<TProcessor, TProcessorInput, TProcessorOutput>(TProcessorInput input, CancellationToken cancellationToken) where TProcessor : IProcessor<TProcessorInput, TProcessorOutput>
        {
            IProcessor<TProcessorInput, TProcessorOutput> processor = _serviceProvider.GetRequiredService<TProcessor>();

            return await processor.ProcessAsync(input, cancellationToken);
        }

        public abstract Task ProcessAsync(CancellationToken cancellationToken);
    }
}
