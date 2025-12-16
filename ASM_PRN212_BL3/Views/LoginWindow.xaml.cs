using System.Windows;
using ASM.Entities.Models;

namespace ASM_PRN212_BL3.Views
{
    /// <summary>
    /// Cua so chon phuong thuc dang nhap
    /// Cho phep chon vai tro: Admin hoac Student
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Dang nhap voi vai tro Admin
        /// </summary>
        private void BtnAdmin_Click(object sender, RoutedEventArgs e)
        {
            var adminUser = new User
            {
                Id = 1,
                Username = "admin",
                DisplayName = "Quan Tri Vien",
                Role = UserRole.Admin
            };

            OpenAdminWindow(adminUser);
        }

        /// <summary>
        /// Dang nhap voi vai tro Student
        /// </summary>
        private void BtnStudent_Click(object sender, RoutedEventArgs e)
        {
            var studentUser = new User
            {
                Id = 2,
                Username = "student",
                DisplayName = "Sinh Vien",
                Role = UserRole.Student
            };

            OpenQuizWindow(studentUser);
        }

        /// <summary>
        /// Mo cua so Admin (MainWindow)
        /// </summary>
        private void OpenAdminWindow(User user)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        /// <summary>
        /// Mo cua so hoc (QuizWindow)
        /// </summary>
        private void OpenQuizWindow(User user)
        {
            var quizWindow = new QuizWindow(user);
            quizWindow.Show();
            this.Close();
        }
    }
}
