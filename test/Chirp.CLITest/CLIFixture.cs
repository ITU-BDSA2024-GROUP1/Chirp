using Chirp.CLI;
using Chirp.Core;

using SimpleDB;

namespace Chirp.CLITest;

public class CLIFixture
{
    public readonly UserInterface UserInterface;

    public CLIFixture()
    {
        DirectoryFixer.SetWorkingDirectoryToProjectRoot();
        
        IDatabaseRepository<Cheep> repo = new CSVDatabase<Cheep>("data/test.csv");
        UserInterface = new UserInterface(repo);
    }
}