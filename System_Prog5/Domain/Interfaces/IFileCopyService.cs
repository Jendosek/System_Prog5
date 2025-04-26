namespace System_Prog5.Domain.Interfaces;

public interface IFileCopyService
{
    void CopyFile(string sourcePath, string destinationPath, int threadCount);
    void Pause();
    void Resume();
    void Stop();
    bool IsStopped { get; }
}