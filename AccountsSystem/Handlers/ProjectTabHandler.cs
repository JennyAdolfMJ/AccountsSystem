using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AccountsSystem
{
    class ProjectTabHandler
    {
        private DataGrid ProjectTable;

        public ProjectTabHandler(DataGrid tab)
        {
            ProjectTable = tab;
            ProjectTable.ItemsSource = ExpenseDBProvider.Instance().getProjects();
        }

        public void Refresh()
        {
            ProjectTable.ItemsSource = ExpenseDBProvider.Instance().getProjects();
        }

        public void Save(string name, string description)
        {
            Project project = new Project();
            project.ProjectName = name;
            project.ProjectDescription = description;
            ExpenseDBProvider.Instance().Add(project);
            ExpenseDBProvider.Instance().Save();
        }

        public void Delete()
        {
            List<Project> projects = new List<Project>();
            foreach(var item in ProjectTable.SelectedItems)
            {
                projects.Add(item as Project);
            }
            ExpenseDBProvider.Instance().RemoveRange(projects);
            ExpenseDBProvider.Instance().Save();
        }
    }
}
