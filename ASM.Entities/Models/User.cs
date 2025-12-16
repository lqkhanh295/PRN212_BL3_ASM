namespace ASM.Entities.Models
{
    /// <summary>
    /// Enum định nghĩa vai trò người dùng
    /// </summary>
    public enum UserRole
    {
        /// <summary>
        /// Sinh viên - chỉ được học và làm quiz
        /// </summary>
        Student,

        /// <summary>
        /// Quản trị viên - được thêm, sửa, xóa flashcard
        /// </summary>
        Admin
    }

    /// <summary>
    /// Model đại diện cho người dùng trong hệ thống
    /// </summary>
    public class User
    {
        /// <summary>
        /// ID duy nhất của người dùng
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Mật khẩu (trong thực tế nên hash)
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Tên hiển thị
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Vai trò của người dùng
        /// </summary>
        public UserRole Role { get; set; } = UserRole.Student;

        /// <summary>
        /// Ngày tạo tài khoản
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}