using Chirp.Core;

using SimpleDB;


namespace Chirp.CSVDBServiceTest;

public class CSVDBFixture
{
    public readonly IDatabaseRepository<Cheep> TestBase;
    public const string Path = "data/test.csv";

    public CSVDBFixture()
    {
        DirectoryFixer.SetWorkingDirectoryToProjectRoot();
        TestBase = new CSVDatabase<Cheep>(Path);
    }
}