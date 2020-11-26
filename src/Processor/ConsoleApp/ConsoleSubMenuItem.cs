using System;

namespace Processor.ConsoleApp
{
    public class ConsoleSubMenuItem : MenuItem
    {
        public Type ProcessorType { get; }

        public ConsoleSubMenuItem(string key, string name, Type processorType)
            : base(key, name)
        {
            ProcessorType = processorType;
        }
    }
}
