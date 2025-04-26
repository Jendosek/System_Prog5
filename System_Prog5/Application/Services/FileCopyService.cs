using System_Prog5.Domain.Interfaces;
using System_Prog5.Infrastructure.FileSystem;

namespace System_Prog5.Application.Services;


    public class FileCopyService : IFileCopyService
    {
        private CancellationTokenSource _cts;
        private ManualResetEventSlim _pauseEvent;
        private bool _isStopped;

        public bool IsStopped => _isStopped;

        public FileCopyService()
        {
            _pauseEvent = new ManualResetEventSlim(true);
            _cts = new CancellationTokenSource();
        }

        public void CopyFile(string sourcePath, string destinationPath, int threadCount)
        {
            _cts = new CancellationTokenSource();
            _pauseEvent = new ManualResetEventSlim(true);
            _isStopped = false;

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
                    var worker = new FileCopyWorker(_pauseEvent);
                    worker.CopyChunk(sourcePath, destinationPath, start, end, (copied) => progress[threadIndex] = copied, _cts.Token);
                }, _cts.Token);
            }

            // Показ прогресу
            Task.Run(() =>
            {
                while (!Task.WhenAll(tasks).IsCompleted)
                {
                    if (_cts.Token.IsCancellationRequested)
                        break;

                    long totalCopied = 0;
                    foreach (var p in progress)
                        totalCopied += p;
                    double percentage = (double)totalCopied / fileSize * 100;
                    Console.Write($"\rProgress: {percentage:F2}%");
                    Thread.Sleep(500);
                }
            }).Wait();

            if (!_cts.IsCancellationRequested)
                Console.WriteLine("\nCopy completed.");
            else
                Console.WriteLine("\nCopy canceled.");
        }

        public void Pause()
        {
            _pauseEvent.Reset();
        }

        public void Resume()
        {
            _pauseEvent.Set();
        }

        public void Stop()
        {
            _isStopped = true;
            _cts.Cancel();
            _pauseEvent.Set(); // Розблокувати всі потоки, які чекають на паузі
        }
    }
