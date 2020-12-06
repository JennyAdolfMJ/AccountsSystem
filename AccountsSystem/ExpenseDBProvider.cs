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

        public static void Add(Expense transaction)
        {
            s_DBProvider.dbContext.Expenses.Add(transaction);
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

        public static List<ExpenseView> getExpenses(bool businessOnly = false)
        {
            IQueryable<Expense> expenses = s_DBProvider.dbContext.Expenses;

            List<ExpenseView> expenseViews = new List<ExpenseView>();
            foreach (Expense expense in expenses.ToList())
            {
                bool isBusiness = s_DBProvider.dbContext.ProjectExpense.Select(t => t.ExpenseID == expense.ID).Count() > 0;

                if (businessOnly && !isBusiness)
                    continue;

                expenseViews.Add(new ExpenseView(expense, isBusiness));
            }

            return expenseViews;
        }

        public static List<Project> getProjects()
        {
            return s_DBProvider.dbContext.Projects.ToList();
        }

        public static List<ProjectExpenseView> getProjExpenses(int? projectID = null)
        {
            IQueryable<ProjectExpense> projectExpenses = s_DBProvider.dbContext.ProjectExpense;

            List<ProjectExpenseView> result = new List<ProjectExpenseView>();
            foreach (ProjectExpense projectExpense in projectExpenses)
            {
                Expense expense = s_DBProvider.dbContext.Expenses.Single(t => t.ID == projectExpense.ExpenseID);

                Project project = null;
                try
                {
                    project = s_DBProvider.dbContext.Projects.Single(t => t.ID == projectExpense.ProjectID);
                }
                catch { }

                result.Add(new ProjectExpenseView(projectExpense, expense, project));
            }

            return result;
        }

        public static ProjectExpense GetProjectExpense(int id)
        {
            return s_DBProvider.dbContext.ProjectExpense.Find(id);
        }

        public static void Reset()
        {
            s_DBProvider.dbContext.Expenses.RemoveRange(s_DBProvider.dbContext.Expenses);
            s_DBProvider.dbContext.ProjectExpense.RemoveRange(s_DBProvider.dbContext.ProjectExpense);
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


