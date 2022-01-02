using Xunit;

namespace LibreStore.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {

    }

    [Fact]
    public void CreateMainKey(){
        Models.MainToken mt = new Models.MainToken("testkey");
        Xunit.Assert.True( mt.Key == "testkey");

    }
}