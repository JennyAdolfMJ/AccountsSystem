using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;

namespace AccountsSystem
{
    [Table(Name = "ProjectExpense")]
    class ProjectExpense
    {
        [Column(Name = "ID", IsDbGenerated = true, IsPrimaryKey = true, DbType = "INTEGER")]
        [Key]
        public int ID { get; set; }

        [Column(Name = "ProjectID", DbType = "INTEGER")]
        public int? ProjectID { get; set; }

        [Column(Name = "Usage", DbType = "TEXT")]
        public string Usage { get; set; }
    }
}
