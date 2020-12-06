using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;

namespace AccountsSystem
{
    class ExpenseView
    {
        public ExpenseView(Expense expense, bool isBusiness)
        {
            ID = expense.ID;
            TimeStamp = expense.TimeStamp;
            Product = expense.Product;
            Source = expense.Source;
            IsBusiness = isBusiness;
        }

        public int ID { get; set; }

        public DateTime TimeStamp { get; set; }

        public string Product { get; set; }

        public float Price { get; set; }

        public string Source { get; set; }

        public bool IsBusiness { get; set; }
    }
}
