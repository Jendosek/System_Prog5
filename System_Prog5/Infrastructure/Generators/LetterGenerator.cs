using System_Prog5.Domain.Interfaces;

namespace System_Prog5.Infrastructure.Generators;

public class LetterGenerator : IGenerator
{
    public void Generate(CancellationToken token)
    {
        Random random = new Random();
        while (!token.IsCancellationRequested)
        {
            char letter = (char)random.Next('A', 'Z' + 1);
            Console.WriteLine($"[Letter] {letter}");
            Thread.Sleep(500);
        }
    }
}