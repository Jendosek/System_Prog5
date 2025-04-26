namespace System_Prog5.Infrastructure.FileSystem;

public class FileCopyWorker
{
    private readonly ManualResetEventSlim _pauseEvent;
    private const int BufferSize = 81920; // 80 KB

    public FileCopyWorker(ManualResetEventSlim pauseEvent)
    {
        _pauseEvent = pauseEvent;
    }

    public void CopyChunk(string sourcePath, string destinationPath, long start, long end, Action<long> progressCallback, CancellationToken token)
    {
        using (var sourceStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        using (var destinationStream = new FileStream(destinationPath, FileMode.Open, FileAccess.Write, FileShare.Write))
        {
            sourceStream.Position = start;
            destinationStream.Position = start;

            var buffer = new byte[BufferSize];
            long totalCopied = 0;

            while (start + totalCopied < end)
            {
                token.ThrowIfCancellationRequested();
                _pauseEvent.Wait(token); // Чекаємо, якщо пауза

                int toRead = (int)Math.Min(BufferSize, end - (start + totalCopied));
                int bytesRead = sourceStream.Read(buffer, 0, toRead);
                if (bytesRead == 0) break;

                destinationStream.Write(buffer, 0, bytesRead);
                totalCopied += bytesRead;
                progressCallback(totalCopied);
            }
        }
    }
}