using Chirp.Core;

using SimpleDB;

namespace SimpleDBTest;

public class SimpleDBFixture
{
    public readonly IDatabaseRepository<Cheep> TestBase;
    public const string Path = "data/test.csv";
    
    public SimpleDBFixture()
    {
        DirectoryFixer.SetWorkingDirectoryToProjectRoot();
        TestBase = new CSVDatabase<Cheep>(Path);
    }
}