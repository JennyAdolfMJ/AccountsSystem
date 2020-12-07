using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;

namespace AccountsSystem
{
    internal class ExpenseView
    {
        public ExpenseView(Expense expense, bool isBusiness)
        {
            ID = expense.ID;
            TimeStamp = expense.TimeStamp;
            Product = expense.Product;
            Price = expense.Price;
            Source = expense.Source;
            IsBusiness = isBusiness;
            Dirty = false;
        }

        public int ID { get; private set; }

        public DateTime TimeStamp { get; private set; }

        public string Product { get; private set; }

        public float Price { get; private set; }

        public string Source { get; private set; }

        public bool IsBusiness { get; set; }

        public bool Dirty { get; set; }

        public ProjectExpense ToProjectExpense()
        {
            ProjectExpense projectExpense = new ProjectExpense();
            projectExpense.ExpenseID = ID;
            return projectExpense;
        }
    }
}
