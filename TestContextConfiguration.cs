using System.Data.Entity;

namespace ConsoleApp1
{
    public class TestContextConfiguration : DbConfiguration
    {
        public TestContextConfiguration()
        {
            SetDatabaseInitializer<TestContext>(null);
            SetManifestTokenResolver(new SqlManualManifestTokenResolver("2012"));
        }
    }
}

