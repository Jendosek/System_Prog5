using System_Prog5.Presentation;

namespace System_Prog5;

class Program
{
    static async Task Main(string[] args)
    {
        var ui = new ConsoleUI();
        await ui.RunAsync();
    }
}
