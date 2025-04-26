namespace System_Prog5.Domain;

public interface IDirectoryCopyService
{
    void CopyDirectory(string sourcePath, string destinationPath, int threadCount);
    void Pause();
    void Resume();
    void Stop();
    bool IsStopped { get; }
}