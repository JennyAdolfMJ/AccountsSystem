using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;

namespace ExpenseAccount
{
    [Table(Name = "ProjectExpense")]
    class ProjectExpense
    {
        [Column(Name = "ID", IsDbGenerated = true, IsPrimaryKey = true, DbType = "INTEGER")]
        [Key]
        public int ID { get; set; }

        [Column(Name = "TransactionID", CanBeNull = false, DbType = "INTEGER")]
        public int TransactionID { get; set; }

        [Column(Name = "ProjectID", CanBeNull = false, DbType = "INTEGER")]
        public int ProjectID { get; set; }

        [Column(Name = "Usage", DbType = "TEXT")]
        public int Usage { get; set; }
    }
}
