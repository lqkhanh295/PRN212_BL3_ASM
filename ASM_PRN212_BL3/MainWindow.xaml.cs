using System.Windows; // Thêm dòng này để nhận diện Window, RoutedEventArgs
using ASM_PRN212_BL3.ViewModels;
using ASM_PRN212_BL3.Views;

namespace ASM_PRN212_BL3
{
    /// <summary>
    /// MainWindow - Cửa sổ chính của ứng dụng
    /// DataContext được gán trong code-behind (MainViewModel)
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }

        /// <summary>
        /// Xu lý đăng xuất
        /// </summary>
        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Owner = this; // Ensure resource context inheritance
            loginWindow.Show();
            this.Hide(); // Use Hide instead of Close to keep resource context alive
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

