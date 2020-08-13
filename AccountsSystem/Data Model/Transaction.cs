using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;

namespace AccountsSystem
{
    [Table(Name = "Transaction")]
    class Transaction
    {
        [Column(Name = "ID", IsDbGenerated = true, IsPrimaryKey = true, DbType = "INTEGER")]
        [Key]
        public int ID { get; set; }

        [Column(Name = "TimeStamp", CanBeNull = false, DbType = "TEXT")]
        public DateTime TimeStamp { get; set; }

        [Column(Name = "Product", CanBeNull =false, DbType = "TEXT")]
        public string Product { get; set; }

        [Column(Name = "Price", CanBeNull = false, DbType = "NUMERIC")]
        public float Price { get; set; }

        [Column(Name = "Source", CanBeNull = false, DbType = "TEXT")]
        public string Source { get; set; }

        [Column(Name = "Business", CanBeNull = false, DbType = "INTEGER")]
        public bool Business { get; set; }
    }
}
