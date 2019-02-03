namespace ConsoleApp1.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ConsoleApp1.TestContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            using (var context = new TestContext())
            {
                this.SetSqlGenerator("System.Data.SqlClient", new IndexedDiscriminatorGenerator(context.GetDiscriminators()));
            }
        }

        protected override void Seed(ConsoleApp1.TestContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
