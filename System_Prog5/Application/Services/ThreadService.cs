using System_Prog5.Domain.Interfaces;

namespace System_Prog5.Application.Services;

public class ThreadService
{
    private Thread _thread;
    private CancellationTokenSource _cts;

    public void Start(IGenerator generator, ThreadPriority priority)
    {
        _cts = new CancellationTokenSource();
        _thread = new Thread(() => generator.Generate(_cts.Token));
        _thread.Priority = priority;
        _thread.IsBackground = true;
        _thread.Start();
    }

    public void Stop()
    {
        _cts?.Cancel();
    }
}