using System.Windows;
using IRC;

namespace YAC_Bot_v2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UserlistDlg _userlistDlg;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (_userlistDlg == null || !_userlistDlg.IsLoaded)
            {
                _userlistDlg = new UserlistDlg();
                _userlistDlg.Show();
            }
        }

		private void BCheckForUpdates_Click(object sender, RoutedEventArgs e)
		{
			Tester.TestIrc();
		}
	}
}
