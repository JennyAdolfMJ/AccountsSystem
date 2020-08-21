using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;

namespace AccountsSystem
{
    class ProjectExpenseModel
    {
        public int ID { get; set; }

        public int? ProjectID { get; set; }

        public string ProjectName { get; set; }

        public string Usage { get; set; }

        public DateTime TimeStamp { get; set; }

        public string Product { get; set; }

        public float Price { get; set; }
    }
}
