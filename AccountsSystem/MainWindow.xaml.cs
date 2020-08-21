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
            var projectNames = ExpenseDBProvider.getProjects();
            projectNames.Insert(0, new Project(projectNames.Count, "未筛选"));
            projectNames.Add(new Project(projectNames.Count, "+新建项目"));
            ProjListCombo.ItemsSource = projectNames;
            ProjListCombo.SelectedIndex = 0;
            UpdateTable();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
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
                MessageBox.Show("Success");
            }
        }

        private void UpdateTable()
        {
            TransTable.ItemsSource = ExpenseDBProvider.getTransactions();
            ProjExpenseTable.ItemsSource = ExpenseDBProvider.getBusinessTrans();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            NewItemDlg window1 = new NewItemDlg();

            if(window1.ShowDialog().Value == true)
            {
                MessageBox.Show(window1.ProjectName);
            }
        }

        private void ProjExpenseTable_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ProjExpenseTable.ItemsSource = ExpenseDBProvider.getBusinessTrans();
        }

        private void TransTable_CellEditEnding(object sender, System.Windows.Controls.DataGridCellEditEndingEventArgs e)
        {
            Transaction transaction = e.Row.Item as Transaction;
            transaction.UpdateBusiness(transaction.ProjectExpenseID);
            ExpenseDBProvider.Save();
        }

        private void ProjExpenseTable_CellEditEnding(object sender, System.Windows.Controls.DataGridCellEditEndingEventArgs e)
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
    }
}
