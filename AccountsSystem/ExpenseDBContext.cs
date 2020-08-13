using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace AccountsSystem
{
    class ExpenseDBContext : DbContext
    {
        public ExpenseDBContext() : base(new SQLiteConnection()
        {
            ConnectionString = new SQLiteConnectionStringBuilder() { DataSource = @"AccountsSystem.db" }.ConnectionString
        }, true)
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<ProjectExpense> ProjectExpense { get; set; }
    }
}


