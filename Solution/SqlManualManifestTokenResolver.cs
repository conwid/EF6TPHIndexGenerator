using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace ConsoleApp1
{
    public class SqlManualManifestTokenResolver : IManifestTokenResolver
    {
        public string ResolvedVersion { get; }
        public SqlManualManifestTokenResolver(string version)
        {
            this.ResolvedVersion = version;
        }
        public string ResolveManifestToken(DbConnection connection) => ResolvedVersion;
    }
}

