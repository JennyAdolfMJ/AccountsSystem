using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;

namespace AccountsSystem
{
    public class ExpensesTabHandler
    {
        private DataGrid ExpenseTable;

        public ExpensesTabHandler(DataGrid tab)
        {
            ExpenseTable = tab;
            ExpenseTable.ItemsSource = ExpenseDBProvider.Instance().getExpenses();
        }

        public void Refresh()
        {
            ExpenseTable.ItemsSource = ExpenseDBProvider.Instance().getExpenses();
        }

        public void TabSelected()
        {
            Refresh();
        }

        public void Save()
        {
            List<ProjectExpense> projectExpenses = new List<ProjectExpense>();
            foreach(ExpenseView expense in ExpenseTable.ItemsSource)
            {
                if (expense.Dirty)
                {
                    ProjectExpense projectExpense = ExpenseDBProvider.Instance().GetProjExpense(expense.ID);

                    if (projectExpense == null && expense.IsBusiness)
                    {
                        ExpenseDBProvider.Instance().Add(expense.ToProjectExpense());
                    }
                    else if (projectExpense != null && !expense.IsBusiness)
                    {
                        ExpenseDBProvider.Instance().Remove(projectExpense);
                    }
                }
            }
            ExpenseDBProvider.Instance().Save();
        }
    }
}
