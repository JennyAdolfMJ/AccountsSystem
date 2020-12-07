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
        private List<TabHandler>  tabHandlers;

        private const int ExpenseTab = 0;
        private const int ProjectTab = 1;
        private const int ProjExpenseTab = 2;

        public MainWindow()
        {
            InitializeComponent();
            expensesTabHandler = new ExpensesTabHandler(ExpenseTable);
            projExpensesTabHandler = new ProjExpensesTabHandler(ProjCombo, ProjExpenseTable);
            projectTabHandler = new ProjectTabHandler(ProjectsTable);
            tabHandlers = new List<TabHandler>();
            tabHandlers.Add(new ExpensesTabHandler(ExpenseTable));
            tabHandlers.Add(new ProjectTabHandler(ProjectsTable));
            tabHandlers.Add(new ProjExpensesTabHandler(ProjCombo, ProjExpenseTable));
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
                    MessageBox.Show("导入成功");
                    ExportBtn.IsEnabled = true;
                    ResetBtn.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show("导入失败");
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

                    switch (tab.TabIndex)
                    {
                        case 0:
                            expensesTabHandler.TabSelected(); break;
                        case 2:
                            projExpensesTabHandler.TabSelected(); break;
                    }
                }
            }
        }

        private void ExportBtn_Click(object sender, RoutedEventArgs e)
        {
            Exporter exporter = new Exporter();

            Forms.SaveFileDialog dialog = new Forms.SaveFileDialog();
            dialog.Filter = "Excel Files|*.xlsx";
            if (dialog.ShowDialog() == Forms.DialogResult.OK)
            {
                exporter.Export(dialog.FileName);
                MessageBox.Show("导出成功");
            }
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            expensesTabHandler.Reset();
            switch(tabControl.SelectedIndex)
            {
                case 0:
                    expensesTabHandler.Refresh(); break;
                case 1:
                    projectTabHandler.Refresh(); break;
                case 2:
                    projExpensesTabHandler.Refresh(); break;
            }
            
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
                    MessageBox.Show("项目已存在");
                    break;
                case Result.Fail:
                    MessageBox.Show("添加失败");
                    break;
            }
        }

        private void ProjDelBtn_Click(object sender, RoutedEventArgs e)
        {
            projectTabHandler.Delete();
            projectTabHandler.Refresh();
        }
    }
}
