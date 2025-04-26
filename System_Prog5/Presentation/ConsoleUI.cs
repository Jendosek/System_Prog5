using System_Prog5.Application.Services;
using System_Prog5.Infrastructure.Generators;

namespace System_Prog5.Presentation;

public class ConsoleUI
{
    private readonly ThreadService _numberService = new();
    private readonly ThreadService _letterService = new();
    private readonly ThreadService _symbolService = new();

    public void Run()
    {
        Console.WriteLine("=== Thread Generator App ===");

        var numberPriority = AskPriority("number");
        var letterPriority = AskPriority("letter");
        var symbolPriority = AskPriority("symbol");

        _numberService.Start(new NumberGenerator(), numberPriority);
        _letterService.Start(new LetterGenerator(), letterPriority);
        _symbolService.Start(new SymbolGenerator(), symbolPriority);

        Console.WriteLine("\nThreads started. Press any key to stop...");
        Console.ReadKey();

        _numberService.Stop();
        _letterService.Stop();
        _symbolService.Stop();

        Console.WriteLine("\nThreads stopped. Press any key to exit.");
        Console.ReadKey();
    }

    private ThreadPriority AskPriority(string generatorName)
    {
        Console.WriteLine($"\nSet priority for {generatorName} generator:");
        foreach (var value in Enum.GetValues(typeof(ThreadPriority)))
        {
            Console.WriteLine($" - {value}");
        }
        Console.Write("Your choice: ");
        string input = Console.ReadLine();

        if (Enum.TryParse<ThreadPriority>(input, true, out var priority))
        {
            return priority;
        }
        else
        {
            Console.WriteLine("Invalid input. Defaulting to Normal priority.");
            return ThreadPriority.Normal;
        }
    }
}