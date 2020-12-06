using System.Collections.Generic;
using System.Windows.Controls;

namespace AccountsSystem
{
    class ProjExpensesTabHandler
    {
        private DataGrid ProjectExpenseTable;

        public ProjExpensesTabHandler(DataGrid table)
        {
            ProjectExpenseTable = table;
            ProjectExpenseTable.ItemsSource = ExpenseDBProvider.Instance().getProjExpenses();
        }

        public void Refresh()
        {
            ProjectExpenseTable.ItemsSource = ExpenseDBProvider.Instance().getProjExpenses();
        }

        public void TabSelected()
        {
            Refresh();
        }

        public void TabUnSelected()
        {

        }
    }
}
