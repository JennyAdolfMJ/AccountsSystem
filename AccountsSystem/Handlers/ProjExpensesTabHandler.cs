using System.Collections.Generic;
using System.Windows.Controls;

namespace AccountsSystem
{
    class ProjExpensesTabHandler : TabHandler
    {
        private ComboBox ProjectCombo;
        private DataGrid ProjectExpenseTable;

        public ProjExpensesTabHandler(ComboBox combo, DataGrid table)
        {
            ProjectCombo = combo;
            ProjectCombo.ItemsSource = ExpenseDBProvider.Instance().getProjects();

            ProjectExpenseTable = table;
            ProjectExpenseTable.ItemsSource = ExpenseDBProvider.Instance().getProjExpenses();
        }

        public override void Refresh()
        {
            ProjectCombo.ItemsSource = ExpenseDBProvider.Instance().getProjects();
            ProjectExpenseTable.ItemsSource = ExpenseDBProvider.Instance().getProjExpenses();
        }

        public void Save(int projectId, string usage)
        {
            foreach (ProjectExpenseView item in ProjectExpenseTable.SelectedItems)
            {
                ProjectExpense projectExpense = ExpenseDBProvider.Instance().GetProjExpense(item.ID);

                projectExpense.ProjectID = projectId;
                projectExpense.Usage = usage;
            }
            ExpenseDBProvider.Instance().Save();
            Refresh();
        }

        public void TabSelected()
        {
            Refresh();
        }
    }
}
