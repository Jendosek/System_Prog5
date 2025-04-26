using System_Prog5.Application.Services;

namespace System_Prog5.Presentation;

public class ConsoleUI
{
    private readonly FileCopyService _fileCopyService = new();

    public void Run()
    {
        Console.WriteLine("=== File Copy App ===");

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

        try
        {
            _fileCopyService.CopyFile(sourcePath, destinationPath, threadCount);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}