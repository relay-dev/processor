using System.Collections.Generic;

namespace Processor.ConsoleApp
{
    public class ConsoleMenuItem : MenuItem
    {
        public List<ConsoleSubMenuItem> Processors { get; }

        public ConsoleMenuItem(string key, string name, List<ConsoleSubMenuItem> processors)
            : base(key, name)
        {
            Processors = processors;
        }
    }
}
