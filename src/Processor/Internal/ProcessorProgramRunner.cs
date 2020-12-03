using Processor.ConsoleApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Processor.Internal
{
    internal class ProcessorProgramRunner
    {
        private readonly IServiceProvider _serviceProvider;

        public ProcessorProgramRunner(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            ConsoleMenu consoleMenu = GetConsoleMenu();

            do
            {
                DisplayMainMenu(consoleMenu);

                ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();

                string selection = consoleKeyInfo.KeyChar.ToString();

                if (selection.ToLower() == "x")
                {
                    break;
                }
                else
                {
                    ConsoleMenuItem consoleMenuItem =
                        consoleMenu.ConsoleMenuItems.SingleOrDefault(s => string.Equals(s.Key, selection, StringComparison.OrdinalIgnoreCase));

                    if (consoleMenuItem == null)
                    {
                        Console.Clear();
                        Console.WriteLine("Invalid selection! Please try again (press any key to continue)");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    else
                    {
                        var subApp = new ProcessorSubProgramRunner(_serviceProvider);

                        await subApp.RunAsync(consoleMenuItem, cancellationToken);
                    }
                }
            } while (true);
        }

        private ConsoleMenu GetConsoleMenu()
        {
            List<Type> processorTypes = new AssemblyScanner()
                .GetTypesWithAttribute<ProcessorAttribute>()
                .OrderBy(t => t.Name)
                .ToList();

            var consoleMenuItems = new List<ConsoleMenuItem>();

            List<string> categories = processorTypes
                .Select(t =>
                {
                    ProcessorAttribute processorAttribute =
                        (ProcessorAttribute)t.GetCustomAttributes(typeof(ProcessorAttribute), true).Single();

                    return processorAttribute.Category ?? GetDefaultCategory(t);
                })
                .OrderBy(s => s)
                .Distinct()
                .ToList();

            for (int i = 0; i < categories.Count; i++)
            {
                string key = (i + 1).ToString();
                string name = categories[i];
                var subMenuItems = new List<ConsoleSubMenuItem>();

                List<Type> processorTypesMatched = processorTypes
                    .Where(t =>
                    {
                        ProcessorAttribute processorAttr =
                            (ProcessorAttribute)t.GetCustomAttributes(typeof(ProcessorAttribute), true).Single();

                        return (processorAttr.Category ?? GetDefaultCategory(t)) == name;
                    })
                    .ToList();

                for (int j = 0; j < processorTypesMatched.Count; j++)
                {
                    Type t = processorTypesMatched[j];

                    ProcessorAttribute processorAttr =
                        (ProcessorAttribute)t.GetCustomAttributes(typeof(ProcessorAttribute), true).Single();

                    string k = processorAttr.Key ?? (j + 1).ToString();
                    string n = processorAttr.Name ?? t.Name;

                    k = ToSingleCharacter(k);

                    var menuItem = new ConsoleSubMenuItem(k, n, t);

                    subMenuItems.Add(menuItem);
                }

                key = ToSingleCharacter(key);

                var consoleMenuItem = new ConsoleMenuItem(key, name, subMenuItems);

                consoleMenuItems.Add(consoleMenuItem);
            }

            return new ConsoleMenu(consoleMenuItems);
        }

        private string GetDefaultCategory(Type t)
        {
            return t.Namespace;
        }

        private string ToSingleCharacter(string k)
        {
            if (!int.TryParse(k, out int key) || key < 10)
            {
                return k;
            }

            int i = key + 87;

            if (i > 122)
            {
                throw new InvalidOperationException("Too many processors detected for the menu. You can have at most 36 processors.");
            }

            return ((char)i).ToString();
        }

        private static void DisplayMainMenu(ConsoleMenu consoleMenu)
        {
            Console.Clear();

            Console.WriteLine("Please make a selection:{0}", Environment.NewLine);

            foreach (ConsoleMenuItem consoleAppMenu in consoleMenu.ConsoleMenuItems)
            {
                Console.WriteLine(" ({0}) {1}", consoleAppMenu.Key, consoleAppMenu.Name);
            }

            Console.WriteLine("{0}Enter {1}{2} (enter 'x' to exit)", Environment.NewLine, consoleMenu.ConsoleMenuItems.Min(s => s.Key), consoleMenu.ConsoleMenuItems.Count == 1 ? string.Empty : " - " + consoleMenu.ConsoleMenuItems.Last().Key);
        }
    }
}
