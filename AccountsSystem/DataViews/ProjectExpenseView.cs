using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;

namespace AccountsSystem
{
    class ProjectExpenseView
    {
        public ProjectExpenseView(ProjectExpense projectExpense, Expense expense, Project project = null)
        {
            ID = projectExpense.ID;
            Usage = projectExpense.Usage;
            TimeStamp = expense.TimeStamp;
            Product = expense.Product;
            Price = expense.Price;

            if(project != null)
            {
                ProjectID = project.ID;
                ProjectName = project.ProjectName;
            }
        }

        public int ID { get; set; }

        public DateTime TimeStamp { get; set; }

        public string Product { get; set; }

        public float Price { get; set; }

        public int? ProjectID { get; set; }

        public string ProjectName { get; set; }

        public string Usage { get; set; }
    }
}
