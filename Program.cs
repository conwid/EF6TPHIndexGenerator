using ConsoleApp1.Migrations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Design;
using System.Data.Entity.Migrations.Infrastructure;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.Migrations.Utilities;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.SqlServer;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public abstract class Person
    {
        public int PersonId { get; set; }
        public string Name { get; set; }
    }

    public class Manager : Person
    {
        public string Degree { get; set; }
    }

    public class Doctor : Person
    {
        public string Speciality { get; set; }
    }

    public class TestContext : DbContext
    {       
        public DbSet<Person> People { get; set; }
      
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().Map<Person>(m => m.Requires("Type").HasValue("P"))
                                         .Map<Manager>(m => m.Requires("Type").HasValue("M"))
                                         .Map<Doctor>(m => m.Requires("Type").HasValue("D"));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {           
            var scriptor = new MigratorScriptingDecorator(new DbMigrator(new Configuration()));
            var script = scriptor.ScriptUpdate(null, null);
            Console.WriteLine(script);
            Console.ReadLine();
        }
    }
}

