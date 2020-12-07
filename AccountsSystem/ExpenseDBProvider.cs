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

        ~ExpenseDBProvider()
        {
            s_DBProvider.dbContext.Dispose();
        }

        public static ExpenseDBProvider Instance()
        {
            if (s_DBProvider == null)
                s_DBProvider = new ExpenseDBProvider();

            return s_DBProvider;
        }

        public void AddRange(List<Expense> expense)
        {
            dbContext.Expenses.AddRange(expense);
        }

        public void Add(Project project)
        {
            dbContext.Projects.Add(project);
        }

        public void Add(ProjectExpense projectExpense)
        {
            dbContext.ProjectExpense.Add(projectExpense);
        }

        public void RemoveRange(List<Project> projects)
        {
            dbContext.Projects.RemoveRange(projects);
        }

        public void Remove(ProjectExpense projectExpense)
        {
            dbContext.ProjectExpense.Remove(projectExpense);
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }

        public List<ExpenseView> getExpenses(bool businessOnly = false)
        {
            IQueryable<Expense> expenses = dbContext.Expenses;

            List<ExpenseView> expenseViews = new List<ExpenseView>();
            foreach (Expense expense in expenses.ToList())
            {
                bool isBusiness = dbContext.ProjectExpense.Where(t => t.ExpenseID == expense.ID).Count() > 0;

                if (businessOnly && !isBusiness)
                    continue;

                expenseViews.Add(new ExpenseView(expense, isBusiness));
            }

            return expenseViews;
        }

        public List<Project> getProjects()
        {
            return dbContext.Projects.ToList();
        }

        public List<ProjectExpenseView> getProjExpenses(int? projectID = null)
        {
            IQueryable<ProjectExpense> projectExpenses = dbContext.ProjectExpense;

            List<ProjectExpenseView> result = new List<ProjectExpenseView>();
            foreach (ProjectExpense projectExpense in projectExpenses)
            {
                Expense expense = dbContext.Expenses.Single(t => t.ID == projectExpense.ExpenseID);

                Project project = null;
                try
                {
                    project = dbContext.Projects.Single(t => t.ID == projectExpense.ProjectID);
                }
                catch { }

                result.Add(new ProjectExpenseView(projectExpense, expense, project));
            }

            return result;
        }

        public ProjectExpense GetProjExpense(int expenseId)
        {
            try
            {
                return dbContext.ProjectExpense.Single(t => t.ExpenseID == expenseId);
            }
            catch { return null; }
        }

        public void Reset()
        {
            dbContext.Expenses.RemoveRange(dbContext.Expenses);
            dbContext.ProjectExpense.RemoveRange(dbContext.ProjectExpense);
            Save();
        }
    }
}


