using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
                }
                else
                {
                    MessageBox.Show("导入失败");
                }
            }
        }

        private void ProjExpenseTable_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if(e.Column is DataGridComboBoxColumn)
            {
                dynamic item = e.Row.Item;
                ComboBox combo = e.EditingElement as ComboBox;
                ProjectExpense projectExpense = ExpenseDBProvider.Instance().GetProjExpenseByExpense(item.ID);
                projectExpense.ProjectID = combo.SelectedIndex;
                ExpenseDBProvider.Instance().Save();
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
            ExpenseDBProvider.Instance().Reset();
            ExpenseTable.ItemsSource = ExpenseDBProvider.Instance().getExpenses();
        }

        private void SaveExpenseBtn_Click(object sender, RoutedEventArgs e)
        {
            expensesTabHandler.Save();
        }

        private void ExpensesTable_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            ExpenseView expenseView = e.Row.Item as ExpenseView;
            expenseView.Dirty = true;
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
