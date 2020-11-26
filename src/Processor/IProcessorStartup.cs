using Microsoft.Extensions.DependencyInjection;

namespace Processor
{
    public interface IProcessorStartup
    {
        void ConfigureServices(IServiceCollection services);
    }
}