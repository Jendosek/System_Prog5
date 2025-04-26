using System_Prog5.Application.Services;

namespace System_Prog5.Presentation;

public class ConsoleUI
    {
        private FileCopyService _fileCopyService;

        public void Run()
        {
            Console.WriteLine("=== File Copy App ===");

            while (true)
            {
                Console.Write("Enter source file path: ");
                string sourcePath = Console.ReadLine();

                Console.Write("Enter destination file path: ");
                string destinationPath = Console.ReadLine();

                Console.Write("Enter number of threads: ");
                if (!int.TryParse(Console.ReadLine(), out int threadCount) || threadCount <= 0)
                {
                    Console.WriteLine("Invalid number of threads. Using 1 thread.");
                    threadCount = 1;
                }

                _fileCopyService = new FileCopyService();

                Task copyTask = Task.Run(() => _fileCopyService.CopyFile(sourcePath, destinationPath, threadCount));
                Task controlTask = Task.Run(() => ListenForControls());

                Task.WaitAny(copyTask, controlTask);

                if (_fileCopyService.IsStopped)
                {
                    Console.WriteLine("\nCopy stopped. Would you like to start again? (y/n)");
                    if (Console.ReadLine()?.ToLower() != "y")
                        break;
                }
                else
                {
                    Console.WriteLine("\nFile copied successfully.");
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
                    _fileCopyService.Pause();
                }
                else if (key == ConsoleKey.R)
                {
                    Console.WriteLine("\nResumed.");
                    _fileCopyService.Resume();
                }
                else if (key == ConsoleKey.S)
                {
                    Console.WriteLine("\nStopping...");
                    _fileCopyService.Stop();
                    break;
                }
            }
        }
    }