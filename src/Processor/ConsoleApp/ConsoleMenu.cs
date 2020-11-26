using System.Collections.Generic;

namespace Processor.ConsoleApp
{
    public class ConsoleMenu
    {
        public List<ConsoleMenuItem> ConsoleMenuItems { get; }

        public ConsoleMenu(List<ConsoleMenuItem> consoleMenuItems)
        {
            ConsoleMenuItems = consoleMenuItems;
        }
    }
}
