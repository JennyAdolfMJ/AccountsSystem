using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;

namespace AccountsSystem
{
    [Table(Name = "Project")]
    class Project
    {
        public Project()
        { }

        public Project(int id, string projectName)
        {
            ID = id;
            ProjectName = projectName;
        }

        [Column(Name = "ID", IsDbGenerated = true, IsPrimaryKey = true, DbType = "INTEGER")]
        [Key]
        public int ID { get; set; }

        [Column(Name = "ProjectName", CanBeNull = false, DbType = "TEXT")]
        public string ProjectName { get; set; }
    }
}
