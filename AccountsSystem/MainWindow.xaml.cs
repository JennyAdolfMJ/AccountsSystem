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

        public MainWindow()
        {
            InitializeComponent();
            expensesTabHandler = new ExpensesTabHandler();
            ExpensesTable.ItemsSource = expensesTabHandler.Expenses;
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

                importer.Import();
                expensesTabHandler.Refresh();
                MessageBox.Show("导入成功");
            }
        }

        private void ProjExpenseTable_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if(e.Column is DataGridComboBoxColumn)
            {
                dynamic item = e.Row.Item;
                ComboBox combo = e.EditingElement as ComboBox;
                ProjectExpense projectExpense = ExpenseDBProvider.Instance().GetProjectExpense(item.ID);
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
            foreach(ProjectExpenseView item in ProjExpenseTable.SelectedItems)
            {
                ProjectExpense projectExpense = ExpenseDBProvider.Instance().GetProjectExpense(item.ID);

                Project project = ProjCombo.SelectedItem as Project;
                projectExpense.ProjectID = project.ID;
                projectExpense.Usage = UsageText.Text;
            }

            ExpenseDBProvider.Instance().Save();
            ProjExpenseTable.ItemsSource = ExpenseDBProvider.Instance().getProjExpenses();
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

                    }
                }
            }

            foreach (var item in e.RemovedItems)
            {
                if (item is TabItem)
                {
                    TabItem tab = item as TabItem;

                    switch (tab.TabIndex)
                    {
                        case 0:
                            expensesTabHandler.TabUnSelected(); break;

                    }
                }
            }
        }

        private void ExportBtn_Click(object sender, RoutedEventArgs e)
        {
            Exporter exporter = new Exporter();
            exporter.Export(ExpenseDBProvider.Instance().getProjExpenses());

            MessageBox.Show("导出成功");
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            ExpenseDBProvider.Instance().Reset();
            ExpenseDBProvider.Instance().Save();
            ExpensesTable.ItemsSource = ExpenseDBProvider.Instance().getExpenses();
        }
    }
}
