namespace Chirp.CLITest;

[Collection("Chirp.CLI Collection")]
public abstract class CLITester : IDisposable
{
    private readonly string[] _originalDBContent;
    private readonly int _originalDBLength;
    
    protected readonly CLIFixture _fixture;

    protected CLITester(CLIFixture fixture)
    {
        _fixture = fixture;

        _originalDBContent = ReadFile();
        _originalDBLength = _originalDBContent.Length;
    }

    public void Dispose()
    {
        var newDBLength = ReadFile().Length;
        if (newDBLength != _originalDBLength)
        {
            File.WriteAllLines(CLIFixture.Path, _originalDBContent);
            newDBLength = ReadFile().Length;
        }
        
        Assert.Equal(_originalDBLength, newDBLength);
    }

    protected static string[] ReadFile() => File.ReadAllLines(CLIFixture.Path);
}