namespace SimpleDBTest;

[Collection("SimpleDB Collection")]
public abstract class SimpleDBTester : IDisposable
{
    private const string Path = "data/test.csv";
    private readonly string[] _originalDBContent;
    protected readonly int _originalDBLength;
    
    protected readonly SimpleDBFixture _fixture;

    protected SimpleDBTester(SimpleDBFixture fixture)
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
            File.WriteAllLines(Path, _originalDBContent);
            newDBLength = ReadFile().Length;
        }
        
        Assert.Equal(_originalDBLength, newDBLength);
    }

    protected static string[] ReadFile() => File.ReadAllLines(Path);
}