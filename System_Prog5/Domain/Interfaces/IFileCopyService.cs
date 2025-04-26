namespace System_Prog5.Domain.Interfaces;

public interface IFileCopyService
{
    void CopyFile(string sourcePath, string destinationPath, int threadCount);
}