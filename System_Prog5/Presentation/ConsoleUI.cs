using System_Prog5.Application;

namespace System_Prog5.Presentation;

public class ConsoleUI
    {
        private DirectoryCopyService _copyService;

        public void Run()
        {
            Console.WriteLine("=== Directory Copy App ===");

            while (true)
            {
                Console.Write("Enter source directory path: ");
                string? sourcePath = Console.ReadLine();

                Console.Write("Enter destination directory path: ");
                string? destinationPath = Console.ReadLine();

                Console.Write("Enter number of threads: ");
                if (!int.TryParse(Console.ReadLine(), out int threadCount) || threadCount <= 0)
                {
                    Console.WriteLine("Invalid number of threads. Using 1 thread.");
                    threadCount = 1;
                }

                _copyService = new DirectoryCopyService();

                Task copyTask = Task.Run(() => _copyService.CopyDirectory(sourcePath, destinationPath, threadCount));
                Task controlTask = Task.Run(() => ListenForControls());

                Task.WaitAny(copyTask, controlTask);

                if (_copyService.IsStopped)
                {
                    Console.WriteLine("\nCopy stopped. Would you like to copy another directory? (y/n)");
                    if (Console.ReadLine()?.ToLower() != "y")
                        break;
                }
                else
                {
                    Console.WriteLine("\nDirectory copied successfully.");
                    break;
                }
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private void ListenForControls()
        {
            Console.WriteLine("Press P to Pause, R to Resume, S to Stop");

            while (true)
            {
                var key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.P)
                {
                    Console.WriteLine("\nPaused.");
                    _copyService.Pause();
                }
                else if (key == ConsoleKey.R)
                {
                    Console.WriteLine("\nResumed.");
                    _copyService.Resume();
                }
                else if (key == ConsoleKey.S)
                {
                    Console.WriteLine("\nStopping...");
                    _copyService.Stop();
                    break;
                }
            }
        }
    }