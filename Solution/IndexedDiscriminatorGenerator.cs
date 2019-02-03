using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.Migrations.Sql;
using System.Data.Entity.SqlServer;
using System.Linq;

namespace ConsoleApp1.Migrations
{
    public class IndexedDiscriminatorGenerator : SqlServerMigrationSqlGenerator
    {
        private List<(string tableName, string discriminatorColumName)> discriminatorList;

        public IndexedDiscriminatorGenerator(List<(string tableName, string discriminatorColumName)> discriminatorList)
        {
            this.discriminatorList = discriminatorList;
        }

        protected override void Generate(CreateTableOperation createTableOperation)
        {
            base.Generate(createTableOperation);            
            if (discriminatorList.Any(l => l.tableName == createTableOperation.Name))
            {
                CreateDiscriminatorIndex(discriminatorList.Single(l => l.tableName == createTableOperation.Name));
            }
        }

        private void CreateDiscriminatorIndex((string tableName, string columnName) discriminatorInfo)
        {
            using (var writer = Writer())
            {
                var str = 
                    $"IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Discriminator' AND object_id = OBJECT_ID(N'{discriminatorInfo.tableName}')) "+
                    $"EXECUTE('CREATE  NONCLUSTERED INDEX [IX_Discriminator] ON {discriminatorInfo.tableName} ({discriminatorInfo.tableName})')";
                writer.WriteLine(str);
                Statement(writer);
            }
        }
    }
}