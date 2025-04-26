namespace System_Prog5.Infrastructure.FileSystem;

public class FileCopyWorker
{
    private const int BufferSize = 81920; 

    public void CopyChunk(string sourcePath, string destinationPath, long start, long end, Action<long> progressCallback)
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