using System.Collections.Generic;
using System.Windows.Controls;

namespace AccountsSystem
{
    public enum Result
    {
        Success,
        Fail,
        AlreadyExist
    }

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

        public Result Save(string name, string description)
        {
            if (ExpenseDBProvider.Instance().findProject(name))
            {
                return Result.AlreadyExist;
            }

            Project project = new Project();
            project.ProjectName = name;
            project.ProjectDescription = description;

            try
            {
                ExpenseDBProvider.Instance().Add(project);
                ExpenseDBProvider.Instance().Save();
                return Result.Success;
            }
            catch
            {
                return Result.Fail;
            }
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
