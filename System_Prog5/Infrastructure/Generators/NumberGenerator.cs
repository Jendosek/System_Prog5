using System_Prog5.Domain.Interfaces;

namespace System_Prog5.Infrastructure.Generators;

public class NumberGenerator : IGenerator
{
    public void Generate(CancellationToken token)
    {
        Random random = new Random();
        while (!token.IsCancellationRequested)
        {
            Console.WriteLine($"[Number] {random.Next(0, 100)}");
            Thread.Sleep(500);
        }
    }
}