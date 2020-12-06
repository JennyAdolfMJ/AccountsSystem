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
        public MainWindow()
        {
            InitializeComponent();
            ExpenseDBProvider.Open();
            ProjCombo.ItemsSource = ExpenseDBProvider.getProjects();
            ProjCombo.SelectedIndex = 0;
            UpdateTable();
        }

        public void InitComponents()
        {

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
                UpdateTable();
                MessageBox.Show("导入成功");
            }
        }

        private void UpdateTable()
        {
            TransTable.ItemsSource = ExpenseDBProvider.getExpenses();
            ProjExpenseTable.ItemsSource = ExpenseDBProvider.getProjExpenses();

        }

        private void ProjExpenseTable_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if(e.Column is DataGridComboBoxColumn)
            {
                dynamic item = e.Row.Item;
                ComboBox combo = e.EditingElement as ComboBox;
                ProjectExpense projectExpense = ExpenseDBProvider.GetProjectExpense(item.ID);
                projectExpense.ProjectID = combo.SelectedIndex;
                ExpenseDBProvider.Save();
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
                ProjectExpense projectExpense = ExpenseDBProvider.GetProjectExpense(item.ID);

                Project project = ProjCombo.SelectedItem as Project;
                projectExpense.ProjectID = project.ID;
                projectExpense.Usage = UsageText.Text;
            }

            ExpenseDBProvider.Save();
            ProjExpenseTable.ItemsSource = ExpenseDBProvider.getProjExpenses();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                TabControl tabControl = e.Source as TabControl;

                if (tabControl.SelectedIndex == 1)
                {
                    ProjExpenseTable.ItemsSource = ExpenseDBProvider.getProjExpenses();
                }
            }
        }

        private void TransTable_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count != 1)
                return;

            DataGridCellInfo cellInfo = e.AddedCells[0];
            if (cellInfo.Column is DataGridCheckBoxColumn)
            {
                Expense trans = cellInfo.Item as Expense;
                //trans.UpdateBusiness();
                ExpenseDBProvider.Save();
                TransTable.ItemsSource = ExpenseDBProvider.getExpenses();
            }
        }

        private void ExportBtn_Click(object sender, RoutedEventArgs e)
        {
            Exporter exporter = new Exporter();
            exporter.Export(ExpenseDBProvider.getProjExpenses());

            MessageBox.Show("导出成功");
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            ExpenseDBProvider.Reset();
            ExpenseDBProvider.Save();
            TransTable.ItemsSource = ExpenseDBProvider.getExpenses();
        }
    }
}
