using Chirp.Core;
using Chirp.CSVDBService;

using SimpleDB;

namespace Chirp.CSVDBServiceTest;

public class UnitTestService : CSVDBServiceTester
{
    public WebService testWebService;
    
    public UnitTestService(CSVDBFixture fixture) : base(fixture)
    {
        testWebService = new CSVDBService.WebService(fixture.TestBase);
    }

    [Fact]
    public void CSVDB_Instantiation()
    {
        //ACT
        Assert.NotNull(testWebService);
    }


}

