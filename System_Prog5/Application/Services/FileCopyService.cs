using System_Prog5.Domain.Interfaces;
using System_Prog5.Infrastructure.FileSystem;

namespace System_Prog5.Application.Services;

public class FileCopyService : IFileCopyService
{
    public void CopyFile(string sourcePath, string destinationPath, int threadCount)
    {
        if (!File.Exists(sourcePath))
            throw new FileNotFoundException("Source file does not exist.");

        long fileSize = new FileInfo(sourcePath).Length;
        long chunkSize = fileSize / threadCount;
        var tasks = new Task[threadCount];
        var progress = new long[threadCount];

        using (var destinationStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.Write))
        {
            destinationStream.SetLength(fileSize);
        }

        for (int i = 0; i < threadCount; i++)
        {
            int threadIndex = i;
            tasks[i] = Task.Run(() =>
            {
                long start = threadIndex * chunkSize;
                long end = (threadIndex == threadCount - 1) ? fileSize : start + chunkSize;
                var worker = new FileCopyWorker();
                worker.CopyChunk(sourcePath, destinationPath, start, end, (copied) => progress[threadIndex] = copied);
            });
        }
        
        Task.Run(() =>
        {
            while (!Task.WhenAll(tasks).IsCompleted)
            {
                long totalCopied = 0;
                foreach (var p in progress)
                    totalCopied += p;
                double percentage = (double)totalCopied / fileSize * 100;
                Console.Write($"\rProgress: {percentage:F2}%");
                Thread.Sleep(500);
            }
        }).Wait();

        Console.WriteLine("\nCopy completed.");
    }
}