using System.Numerics;


namespace System_Prog5;

public class CalculateStudy
{
    public static void Run()
    {
        RunAsync();
    }

    public static async Task RunAsync()
    {
        Console.Write("Enter a number: ");
        if (int.TryParse(Console.ReadLine(), out int number) && number >= 0)
        {
            BigInteger result = await Calculate.CalculateFactorialAsync(number);
            Console.WriteLine($"Factorial of {number} is {result}");
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a non-negative integer.");
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}