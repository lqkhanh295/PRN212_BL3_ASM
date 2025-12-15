namespace ASM.Entities.Models
{
    /// <summary>
    /// Enum ??nh ngh?a vai trò ng??i dùng
    /// </summary>
    public enum UserRole
    {
        /// <summary>
        /// Sinh viên - ch? ???c h?c và làm quiz
        /// </summary>
        Student,

        /// <summary>
        /// Qu?n tr? viên - ???c thêm, s?a, xóa flashcard
        /// </summary>
        Admin
    }

    /// <summary>
    /// Model ??i di?n cho ng??i dùng trong h? th?ng
    /// </summary>
    public class User
    {
        /// <summary>
        /// ID duy nh?t c?a ng??i dùng
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Tên ??ng nh?p
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// M?t kh?u (trong th?c t? nên hash)
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Tên hi?n th?
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Vai trò c?a ng??i dùng
        /// </summary>
        public UserRole Role { get; set; } = UserRole.Student;

        /// <summary>
        /// Ngày t?o tài kho?n
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
