namespace System_Prog5.Domain.Interfaces;

public interface IGenerator
{
    void Generate(CancellationToken token);
}