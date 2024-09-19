using Chirp.CLI;
using Chirp.Core;

using SimpleDB;

namespace Chirp.CLITest;

public class CLIFixture
{
    public readonly UserInterface UserInterface;
    public readonly IDatabaseRepository<Cheep> CheepRepository;
    public const string Path = "data/test.csv";

    public CLIFixture()
    {
        DirectoryFixer.SetWorkingDirectoryToProjectRoot();
        
        CheepRepository = new CSVDatabase<Cheep>(Path);
        UserInterface = new UserInterface(CheepRepository);
    }
}