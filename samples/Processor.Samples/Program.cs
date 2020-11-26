using System.Threading.Tasks;

namespace Processor.Samples
{
    class Program : ProcessorProgram<Startup>
    {
        static async Task Main(string[] args)
        {
            await RunAsync();
        }
    }
}
