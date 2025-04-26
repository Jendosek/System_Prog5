namespace System_Prog5.Infrastructure;

public class FileCopyWorker
{
    private readonly ManualResetEventSlim _pauseEvent;
    private const int BufferSize = 81920; // 80 KB

    public FileCopyWorker(ManualResetEventSlim pauseEvent)
    {
        _pauseEvent = pauseEvent;
    }

    public void CopyFile(string sourcePath, string destinationPath, CancellationToken token)
    {
        using (var sourceStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        using (var destStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.Write))
        {
            var buffer = new byte[BufferSize];
            int bytesRead;

            while ((bytesRead = sourceStream.Read(buffer, 0, BufferSize)) > 0)
            {
                token.ThrowIfCancellationRequested();
                _pauseEvent.Wait(token);

                destStream.Write(buffer, 0, bytesRead);
            }
        }
    }
}