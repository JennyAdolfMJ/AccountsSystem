using System.Windows;
using System.Windows.Media;

namespace ExpenseAccount
{
    /// <summary>
    /// Interaction logic for NewItemDlg.xaml
    /// </summary>
    public partial class NewItemDlg : Window
    {
        public string ProjectName;

        public NewItemDlg()
        {
            InitializeComponent();
        }

        private void Ok_btn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace( proj_name.Text))
            {
                proj_name.Background = Brushes.Red;
                return;
            }
            ProjectName = proj_name.Text.Trim();
            this.DialogResult = true;
        }
    }
}
