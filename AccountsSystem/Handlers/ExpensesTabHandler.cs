using System.Collections.Generic;
using System.Windows.Controls;

namespace AccountsSystem
{
    public class ExpensesTabHandler
    {
        internal List<ExpenseView> Expenses { get; private set; }

        public ExpensesTabHandler()
        {
            Expenses = ExpenseDBProvider.Instance().getExpenses();
        }

        public void Refresh()
        {
            Expenses = ExpenseDBProvider.Instance().getExpenses();
        }

        public void TabSelected()
        {

        }
        public void TabUnSelected()
        {

        }
    }
}
