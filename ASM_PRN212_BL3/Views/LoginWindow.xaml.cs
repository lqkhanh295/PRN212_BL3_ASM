using System.Windows;
using ASM.Entities.Models;

namespace ASM_PRN212_BL3.Views
{
    /// <summary>
<<<<<<< HEAD
    /// Cửa sổ chọn phương thức đăng nhập
    /// Cho phép chọn vai trò: Admin, Student, hoặc Guest
=======
    /// Cua so chon phuong thuc dang nhap
    /// Cho phep chon vai tro: Admin hoac Student
>>>>>>> 5b835f85684f5e91133de1435d46ffa8ac8bc8b7
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Đăng nhập với vai trò Admin
        /// </summary>
        private void BtnAdmin_Click(object sender, RoutedEventArgs e)
        {
            var adminUser = new User
            {
                Id = 1,
                Username = "admin",
                DisplayName = "Quản Trị Viên",
                Role = UserRole.Admin
            };

            OpenAdminWindow(adminUser);
        }

        /// <summary>
        /// Đăng nhập với vai trò Student
        /// </summary>
        private void BtnStudent_Click(object sender, RoutedEventArgs e)
        {
            var studentUser = new User
            {
                Id = 2,
                Username = "student",
                DisplayName = "Sinh Viên",
                Role = UserRole.Student
            };

            OpenQuizWindow(studentUser);
        }

        /// <summary>
<<<<<<< HEAD
        /// Vào chế độ Khách (Guest)
        /// </summary>
        private void BtnGuest_Click(object sender, RoutedEventArgs e)
        {
            var guestUser = new User
            {
                Id = 0,
                Username = "guest",
                DisplayName = "Khách",
                Role = UserRole.Student
            };

            OpenQuizWindow(guestUser);
        }

        /// <summary>
        /// Mở cửa sổ Admin (MainWindow)
=======
        /// Mo cua so Admin (MainWindow)
>>>>>>> 5b835f85684f5e91133de1435d46ffa8ac8bc8b7
        /// </summary>
        private void OpenAdminWindow(User user)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        /// <summary>
        /// Mở cửa sổ học (QuizWindow)
        /// </summary>
        private void OpenQuizWindow(User user)
        {
            var quizWindow = new QuizWindow(user);
            quizWindow.Show();
            this.Close();
        }
    }
}