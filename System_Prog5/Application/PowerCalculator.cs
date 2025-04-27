using System_Prog5.Domain;

namespace System_Prog5.Application;

public class PowerCalculator : IPowerCalculator
{
    public async Task<double> CalculatePowerAsync(double baseNumber, int exponent)
    {
        await Task.Yield(); // мінімальна асинхронність
        double result = 1;
        for (int i = 0; i < exponent; i++)
        {
            result *= baseNumber;
        }
        return result;
    }
}