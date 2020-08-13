using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;

namespace ExpenseAccount
{
    [Table(Name = "Project")]
    class Project
    {
        [Column(Name = "ID", IsDbGenerated = true, IsPrimaryKey = true, DbType = "INTEGER")]
        [Key]
        public int ID { get; set; }

        [Column(Name = "ProjectName", CanBeNull = false, DbType = "TEXT")]
        public string ProjectName { get; set; }
    }
}
