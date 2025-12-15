using System.Windows;
using ASM_PRN212_BL3.Views;

namespace ASM_PRN212_BL3
{
    /// <summary>
    /// MainWindow - Cửa sổ chính của ứng dụng
    /// DataContext được gán trong XAML (MainViewModel)
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Xu lý đăng xuất
        /// </summary>
        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}