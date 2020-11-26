using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Processor.ConsoleApp
{
    public class ProcessorConsoleSubApp
    {
        private readonly IServiceProvider _serviceProvider;

        public ProcessorConsoleSubApp(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task RunAsync(ConsoleMenuItem consoleMenuItem, CancellationToken cancellationToken)
        {
            bool isExit = false;

            while (!isExit)
            {
                DisplaySubMenu(consoleMenuItem);

                ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();

                string selection = consoleKeyInfo.KeyChar.ToString();

                if (selection.ToLower() == "h")
                {
                    isExit = true;
                }
                else
                {
                    ConsoleSubMenuItem consoleSubMenuItem =
                        consoleMenuItem.Processors.SingleOrDefault(s => string.Equals(s.Key, selection, StringComparison.OrdinalIgnoreCase));

                    if (consoleSubMenuItem == null)
                    {
                        Console.Clear();
                        Console.WriteLine("Invalid selection! Please try again (press any key to continue)");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    else
                    {
                        Console.Clear();

                        object processor = _serviceProvider.GetService(consoleSubMenuItem.ProcessorType);

                        if (!processor.GetType().GetInterfaces().Contains(typeof(IProcessor<,>)) && !processor.GetType().GetInterfaces().Contains(typeof(IProcessor<>)) && !processor.GetType().GetInterfaces().Contains(typeof(IProcessor)))
                        {
                            Console.WriteLine("Error! That selection is not a processor (press any key to continue)");
                            Console.ReadKey();
                            Console.Clear();
                        }
                        else if (processor.GetType().GetInterfaces().Contains(typeof(IProcessor<,>)) || processor.GetType().GetInterfaces().Contains(typeof(IProcessor<>)))
                        {
                            Try(async () =>
                            {
                                Type inputType = processor.GetType().GenericTypeArguments[0];

                                object input = Activator.CreateInstance(inputType);

                                await ((dynamic)processor).ProcessAsync(input, cancellationToken);
                            });
                        }
                        else
                        {
                            Try(async () =>
                            {
                                await ((IProcessor)processor).ProcessAsync(cancellationToken);
                            });
                        }
                    }
                }
            }
        }

        private static void Try(Action action)
        {
            try
            {
                action.Invoke();

                Console.WriteLine();
                Console.WriteLine("***Complete***");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine($"ERROR: {e.Message}");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
            }
        }

        private static void DisplaySubMenu(ConsoleMenuItem consoleMenuItem)
        {
            Console.Clear();

            Console.WriteLine("Please make a selection: {0}", Environment.NewLine);

            foreach (ConsoleSubMenuItem processor in consoleMenuItem.Processors)
            {
                Console.WriteLine(" ({0}) {1}", processor.Key, processor.Name);
            }

            Console.WriteLine("{0}Enter {1}{2} (enter ( h ) to return to the home menu)", Environment.NewLine, consoleMenuItem.Processors.Min(s => s.Key), consoleMenuItem.Processors.Count == 1 ? string.Empty : " - " + consoleMenuItem.Processors.Max(sm => sm.Key));
        }
    }
}
