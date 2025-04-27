using System.Numerics;

namespace System_Prog5;

public class Calculate
{
    public static async Task<BigInteger> CalculateFactorialAsync(int n)
    {
        BigInteger factorial = 1;
        for (int i = 2; i <= n; i++)
        {
            factorial *= i;
        }
        await Task.Yield();
        return factorial;
    }
}