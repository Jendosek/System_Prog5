using System_Prog5.Application;

namespace System_Prog5.Presentation;

public class ConsoleUI
{
    public async Task RunAsync()
    {
        Console.WriteLine("=== Power Calculator ===");

        Console.Write("Enter base number: ");
        if (!double.TryParse(Console.ReadLine(), out double baseNumber))
        {
            Console.WriteLine("Invalid base number.");
            return;
        }

        Console.Write("Enter exponent (integer): ");
        if (!int.TryParse(Console.ReadLine(), out int exponent))
        {
            Console.WriteLine("Invalid exponent.");
            return;
        }

        var calculator = new PowerCalculator();
        double result = await calculator.CalculatePowerAsync(baseNumber, exponent);

        Console.WriteLine($"{baseNumber} raised to the power of {exponent} is {result}");

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}