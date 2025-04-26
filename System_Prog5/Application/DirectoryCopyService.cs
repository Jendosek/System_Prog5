using System.Collections.Concurrent;
using System_Prog5.Domain;
using System_Prog5.Infrastructure;

namespace System_Prog5.Application;

public class DirectoryCopyService : IDirectoryCopyService
    {
        private CancellationTokenSource _cts;
        private ManualResetEventSlim _pauseEvent;
        private bool _isStopped;
        private int _totalFiles;
        private int _copiedFiles;

        public bool IsStopped => _isStopped;

        public DirectoryCopyService()
        {
            _pauseEvent = new ManualResetEventSlim(true);
            _cts = new CancellationTokenSource();
        }

        public void CopyDirectory(string sourcePath, string destinationPath, int threadCount)
        {
            _cts = new CancellationTokenSource();
            _pauseEvent = new ManualResetEventSlim(true);
            _isStopped = false;
            _copiedFiles = 0;

            if (!Directory.Exists(sourcePath))
                throw new DirectoryNotFoundException("Source directory does not exist.");

            Directory.CreateDirectory(destinationPath);

            var allFiles = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories);
            _totalFiles = allFiles.Length;

            var fileQueue = new ConcurrentQueue<string>(allFiles);

            var tasks = new Task[threadCount];

            for (int i = 0; i < threadCount; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    var worker = new FileCopyWorker(_pauseEvent);
                    while (fileQueue.TryDequeue(out string file))
                    {
                        _pauseEvent.Wait(_cts.Token);
                        if (_cts.Token.IsCancellationRequested)
                            break;

                        string relativePath = Path.GetRelativePath(sourcePath, file);
                        string destFile = Path.Combine(destinationPath, relativePath);
                        Directory.CreateDirectory(Path.GetDirectoryName(destFile));

                        worker.CopyFile(file, destFile, _cts.Token);
                        Interlocked.Increment(ref _copiedFiles);
                    }
                }, _cts.Token);
            }

            // Прогрес
            Task.Run(() =>
            {
                while (!Task.WhenAll(tasks).IsCompleted)
                {
                    if (_cts.Token.IsCancellationRequested)
                        break;

                    double percentage = (double)_copiedFiles / _totalFiles * 100;
                    Console.Write($"\rProgress: {percentage:F2}% ({_copiedFiles}/{_totalFiles})");
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
            _pauseEvent.Set();
        }
    }