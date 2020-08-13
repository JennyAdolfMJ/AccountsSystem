using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data;

namespace AccountsSystem
{
    class ExpenseDBProvider 
    {
        private static ExpenseDBProvider s_DBProvider;
        private ExpenseDBContext dbContext;

        private ExpenseDBProvider()
        {
            dbContext = new ExpenseDBContext();
        }

        private static ExpenseDBProvider instance()
        {
            if (s_DBProvider == null)
                s_DBProvider = new ExpenseDBProvider();

            return s_DBProvider;
        }

        public static void Add(Transaction transaction)
        {
            s_DBProvider.dbContext.Transaction.Add(transaction);
        }

        public static void Save()
        {
            s_DBProvider.dbContext.SaveChanges();
        }

        public static List<Transaction> getTransactions(bool businessOnly = false)
        {
            if (businessOnly)
                return s_DBProvider.dbContext.Transaction.Where(t => t.Business == true).ToList();
            else
                return s_DBProvider.dbContext.Transaction.ToList();
        }

        public static List<Transaction> getTransactions(string projectName)
        {
            Project project = s_DBProvider.dbContext.Project.Where(t => t.ProjectName.Equals(projectName)).Single();
            var transactionIDs =  s_DBProvider.dbContext.ProjectExpense.Where(t => t.ProjectID == project.ID).Select(t => t.TransactionID);

            return s_DBProvider.dbContext.Transaction.Where(t => transactionIDs.Contains(t.ID)).ToList();
        }

        public static List<Project> getProjects()
        {
            return s_DBProvider.dbContext.Project.ToList();
        }

        public static List<string> getProjectNames()
        {
            var projects = getProjects();
            List<string> projectnames = new List<string>();
            foreach(Project project in projects)
            {
                projectnames.Add(project.ProjectName);
            }

            return projectnames;
        }

        public static List<object> getProjectExpense()
        {
            //s_DBProvider.dbContext.ProjectExpense.Include.Join(s_DBProvider.dbContext.Project, expense => expense.ProjectID, proj => proj.ID,
            //    (expense, proj) =>
            //    {
            //    });
            return null;
        }

        public static void Open()
        {
            instance();
        }

        public static void Close()
        {
            s_DBProvider.dbContext.Dispose();
        }
    }
}


