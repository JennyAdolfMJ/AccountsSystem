using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Forms = System.Windows.Forms;

namespace AccountsSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ExpensesTabHandler expensesTabHandler;
        private ProjectTabHandler projectTabHandler;
        private ProjExpensesTabHandler projExpensesTabHandler;

        public MainWindow()
        {
            InitializeComponent();
            expensesTabHandler = new ExpensesTabHandler(ExpenseTable);
            projExpensesTabHandler = new ProjExpensesTabHandler(ProjCombo, ProjExpenseTable);
            projectTabHandler = new ProjectTabHandler(ProjectsTable);

            ResetBtn.IsEnabled = ExpenseTable.Items.Count > 0;
        }

        private void ImportBtn_Click(object sender, RoutedEventArgs e)
        {
            Forms.OpenFileDialog dialog = new Forms.OpenFileDialog();
            dialog.Filter = "Text Files|*.csv";
            if( dialog.ShowDialog() == Forms.DialogResult.OK)
            {
                Importer importer;
                if (comboBox.SelectedIndex == 0)
                    importer = new WeChatImporter(dialog.FileName);
                else
                    importer = new AlipayImporter(dialog.FileName);

                if (importer.Import())
                {
                    expensesTabHandler.Refresh();
                    ShowMessage("导入成功");
                    ResetBtn.IsEnabled = true;
                }
                else
                {
                    ShowMessage("导入失败", MessageBoxImage.Error);
                }
            }
        }

        private void ProjExpenseTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            EditGroup.IsEnabled = dataGrid.SelectedItems.Count > 0;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            Project project = ProjCombo.SelectedItem as Project;
            projExpensesTabHandler.Save(project.ID, UsageText.Text);
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach(var item in e.AddedItems)
            {
                if(item is TabItem)
                {
                    TabItem tab = item as TabItem;
                    GetTabHandler(tab.TabIndex).Refresh();
                }
            }
        }

        private void ExportBtn_Click(object sender, RoutedEventArgs e)
        {
            Exporter exporter = new Exporter();

            Forms.SaveFileDialog dialog = new Forms.SaveFileDialog();
            dialog.Filter = "Excel Files|*.xlsx";

            if (!exporter.CanExport())
            {
                ShowMessage("没有数据可以导出", MessageBoxImage.Warning);
                return;
            }

            if (dialog.ShowDialog() == Forms.DialogResult.OK)
            {
                Exporter.Result result = exporter.Export(dialog.FileName);

                switch (result)
                {
                    case Exporter.Result.Success:
                        ShowMessage("导出成功"); break;
                    case Exporter.Result.Fail:
                        ShowMessage("导出失败", MessageBoxImage.Error); break;
                }
            }
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            expensesTabHandler.Reset();
            GetTabHandler(tabControl.SelectedIndex).Refresh();
            ResetBtn.IsEnabled = false;
        }

        private void SaveExpenseBtn_Click(object sender, RoutedEventArgs e)
        {
            expensesTabHandler.Save();
            SaveExpenseBtn.IsEnabled = false;
        }

        private void ExpensesTable_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            ExpenseView expenseView = e.Row.Item as ExpenseView;
            expenseView.Dirty = true;
            SaveExpenseBtn.IsEnabled = true;
        }

        private void ProjSaveBtn_Click(object sender, RoutedEventArgs e)
        {
           Result result = projectTabHandler.Save(ProjNameTxt.Text, ProjDescTxt.Text);

            switch(result)
            {
                case Result.Success:
                    projectTabHandler.Refresh();
                    ProjNameTxt.Clear();
                    ProjDescTxt.Clear();
                    break;
                case Result.AlreadyExist:
                    ShowMessage("项目已存在", MessageBoxImage.Warning);
                    break;
                case Result.Fail:
                    ShowMessage("添加失败", MessageBoxImage.Error);
                    break;
            }
        }

        private void ProjDelBtn_Click(object sender, RoutedEventArgs e)
        {
            projectTabHandler.Delete();
            projectTabHandler.Refresh();
        }

        private TabHandler GetTabHandler(int tabIndex)
        {
            switch (tabIndex)
            {
                case 0:
                    return expensesTabHandler;
                case 1:
                    return projectTabHandler;
                case 2:
                    return projExpensesTabHandler;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private void ShowMessage(string message, MessageBoxImage image = MessageBoxImage.Information)
        {
            MessageBox.Show(this, message, "报销管理", MessageBoxButton.OK, image);
        }
    }
}
