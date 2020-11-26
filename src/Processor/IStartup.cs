using Microsoft.Extensions.DependencyInjection;

namespace Processor
{
    public interface IStartup
    {
        void ConfigureServices(IServiceCollection services);
    }
}