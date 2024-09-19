using Chirp.CLI;
using Chirp.Core;

namespace Chirp.CLITest;

public class CLIFixture : IDisposable
{
    private readonly WebDB<Cheep> _testBase = WebDB<Cheep>.Instance;

    public CLIFixture()
    {
        DirectoryFixer.SetWorkingDirectoryToProjectRoot();
        _testBase.TestingDatabase = true;

        new Thread(() => CSVDBService.WebService.Main(Array.Empty<string>())).Start();
        Thread.Sleep(1000);
    }

    public void Dispose()
    {
        _testBase.TestingDatabase = false;
        CSVDBService.WebService.CTS.Cancel();
    }
}