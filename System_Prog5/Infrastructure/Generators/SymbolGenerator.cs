using System_Prog5.Domain.Interfaces;

namespace System_Prog5.Infrastructure.Generators;

public class SymbolGenerator : IGenerator
{
    private readonly char[] symbols = { '@', '#', '$', '%', '&', '*', '!' };

    public void Generate(CancellationToken token)
    {
        Random random = new Random();
        while (!token.IsCancellationRequested)
        {
            char symbol = symbols[random.Next(symbols.Length)];
            Console.WriteLine($"[Symbol] {symbol}");
            Thread.Sleep(500);
        }
    }
}