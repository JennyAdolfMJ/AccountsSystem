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

        public static void Add(ProjectExpense projectExpense)
        {
            s_DBProvider.dbContext.ProjectExpense.Add(projectExpense);
        }

        public static void Remove<ProjectExpense>(int id)
        {
            var projectExpense = s_DBProvider.dbContext.ProjectExpense.Find(id);
            s_DBProvider.dbContext.ProjectExpense.Remove(projectExpense);
        }

        public static void Save()
        {
            s_DBProvider.dbContext.SaveChanges();
        }

        public static List<Transaction> getTransactions(bool businessOnly = false)
        {
            IQueryable<Transaction> result = s_DBProvider.dbContext.Transaction;

            if (businessOnly)
                result = result.Where(t => t.ProjectExpenseID.HasValue);

            return result.ToList();
        }

        public static List<Project> getProjects()
        {
            return s_DBProvider.dbContext.Project.ToList();
        }

        public static List<ProjectExpenseModel> getBusinessTrans(int? projectID = null)
        {
            IQueryable<ProjectExpense> projectExpense = s_DBProvider.dbContext.ProjectExpense;
            if (projectID.HasValue)
                projectExpense = projectExpense.Where(t => t.ProjectID == projectID);

            IQueryable<ProjectExpenseModel> result = projectExpense.Join(s_DBProvider.dbContext.Transaction, expense => expense.ID, trans => trans.ProjectExpenseID,
                (expense, trans) => new ProjectExpenseModel
                {
                    ID = expense.ID,
                    ProjectID = expense.ProjectID,
                    Usage = expense.Usage,
                    TimeStamp = trans.TimeStamp,
                    Product = trans.Product,
                    Price = trans.Price,
                });

            result.GroupJoin(s_DBProvider.dbContext.Project, expense => expense.ProjectID, proj => proj.ID,
                (expense, proj) => new ProjectExpenseModel
                {
                    ID = expense.ID,
                    ProjectID = expense.ProjectID,
                    Usage = expense.Usage,
                    TimeStamp = expense.TimeStamp,
                    Product = expense.Product,
                    Price = expense.Price,
                    ProjectName = proj.FirstOrDefault(t => t.ID == expense.ProjectID).ProjectName
                });

            return result.ToList<ProjectExpenseModel>();
        }

        public static ProjectExpense GetProjectExpense(int id)
        {
            return s_DBProvider.dbContext.ProjectExpense.Find(id);
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


