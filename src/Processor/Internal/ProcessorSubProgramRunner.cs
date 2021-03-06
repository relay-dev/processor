﻿using Microsoft.Extensions.DependencyInjection;
using Processor.ConsoleApp;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Processor.Internal
{
    internal class ProcessorSubProgramRunner
    {
        private readonly IServiceProvider _serviceProvider;

        public ProcessorSubProgramRunner(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task RunAsync(ConsoleMenuItem consoleMenuItem, CancellationToken cancellationToken)
        {
            do
            {
                DisplaySubMenu(consoleMenuItem);

                ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();

                string selection = consoleKeyInfo.KeyChar.ToString();

                if (selection.ToLower() == "h")
                {
                    break;
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

                        IServiceProvider scopedServiceProvider = _serviceProvider.CreateScope().ServiceProvider;

                        object processor = scopedServiceProvider.GetService(consoleSubMenuItem.ProcessorType);

                        if (!processor.GetType().GetInterfaces().Contains(typeof(IProcessor<,>)) && !processor.GetType().GetInterfaces().Contains(typeof(IProcessor<>)) && !processor.GetType().GetInterfaces().Contains(typeof(IProcessor)))
                        {
                            Console.WriteLine("Error! That selection is not a processor (press any key to continue)");
                            Console.ReadKey();
                            Console.Clear();
                        }
                        else if (processor.GetType().GetInterfaces().Contains(typeof(IProcessor<,>)) || processor.GetType().GetInterfaces().Contains(typeof(IProcessor<>)))
                        {
                            InitializeProcessor(processor, scopedServiceProvider);

                            Type inputType = processor.GetType().GenericTypeArguments[0];

                            object input = Activator.CreateInstance(inputType);

                            await ExecuteAsync(async () =>
                            {
                                await ((dynamic)processor).ProcessAsync(input, cancellationToken);
                            });
                        }
                        else
                        {
                            InitializeProcessor(processor, scopedServiceProvider);

                            await ExecuteAsync(async () =>
                            {
                                await ((IProcessor)processor).ProcessAsync(cancellationToken);
                            });
                        }

                        Console.WriteLine();
                        Console.WriteLine("***Complete***");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                    }
                }
            } while (true);
        }

        public async Task ExecuteAsync(Func<Task> process)
        {
            try
            {
                await process();
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine($"ERROR: {e.Message}");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
            }
        }

        private void InitializeProcessor(object processor, IServiceProvider serviceProvider)
        {
            if (processor.GetType().IsSubclassOf(typeof(ProcessorBase)))
            {
                ((ProcessorBase)processor).Initialize(serviceProvider);
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

            Console.WriteLine("{0}Enter {1}{2} (enter 'h' to return to the home menu)", Environment.NewLine, consoleMenuItem.Processors.Min(s => s.Key), consoleMenuItem.Processors.Count == 1 ? string.Empty : " - " + consoleMenuItem.Processors.Last().Key);
        }
    }
}
