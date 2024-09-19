using Chirp.Core;

using SimpleDB;

namespace SimpleDBTest;

public class SimpleDBFixture : IDisposable
{
    public readonly CSVDatabase<Cheep> TestBase = CSVDatabase<Cheep>.Instance;
    
    public SimpleDBFixture()
    {
        DirectoryFixer.SetWorkingDirectoryToProjectRoot();
        TestBase.InTestingDatabase = true;
    }

    public void Dispose()
    {
        TestBase.InTestingDatabase = false;
    }
}