namespace System_Prog5.Domain;

public interface IPowerCalculator
{
    Task<double> CalculatePowerAsync(double baseNumber, int exponent);
}