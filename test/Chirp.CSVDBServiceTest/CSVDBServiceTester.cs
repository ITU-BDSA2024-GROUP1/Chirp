
namespace Chirp.CSVDBServiceTest;

[Collection("CSVDBServiceTest")]

public abstract class CSVDBServiceTester : IDisposable
{
    private readonly string[] _originalCSVcontent;
    private readonly int _originalCSVlength;
    protected readonly CSVDBFixture _fixture;

    protected CSVDBServiceTester(CSVDBFixture fixture)
    {
        _fixture = fixture;
        _originalCSVcontent = ReadFile();
        _originalCSVlength = _originalCSVcontent.Length;
    }
    
    public void Dispose()
    {
        var newDBLength = ReadFile().Length;
        if (newDBLength != _originalCSVlength)
        {
            File.WriteAllLines(CSVDBFixture.Path, _originalCSVcontent);
            newDBLength = ReadFile().Length;
        }
        
        Assert.Equal(_originalCSVlength, newDBLength);
    }

    protected static string[] ReadFile() => File.ReadAllLines(CSVDBFixture.Path);
}

