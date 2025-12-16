using ASM.Data.Repositories;
using ASM.Entities.Models;

namespace ASM.Bussiness.Services
{
    /// <summary>
    /// Service xử lý nghiệp vụ liên quan đến người dùng
    /// </summary>
    public class UserService
    {
        private readonly UserRepository _userRepository;

        // Người dùng hiện tại đang đăng nhập (static để dùng chung)
        private static User? _currentUser;

        public UserService()
        {
            _userRepository = new UserRepository();
        }

        /// <summary>
        /// Lấy người dùng hiện tại
        /// </summary>
        public static User? CurrentUser => _currentUser;

        /// <summary>
        /// Kiểm tra người dùng hiện tại có phải Admin không
        /// </summary>
        public static bool IsAdmin => _currentUser?.Role == UserRole.Admin;

        /// <summary>
        /// Kiểm tra người dùng hiện tại có phải Student không
        /// </summary>
        public static bool IsStudent => _currentUser?.Role == UserRole.Student;

        /// <summary>
        /// Đăng nhập vào hệ thống
        /// </summary>
        /// <param name="username">Tên đăng nhập</param>
        /// <param name="password">Mật khẩu</param>
        /// <returns>User nếu thành công, null nếu thất bại</returns>
        public User? Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            var user = _userRepository.Authenticate(username, password);
            if (user != null)
            {
                _currentUser = user;
            }

            return user;
        }

        /// <summary>
        /// Đăng xuất khỏi hệ thống
        /// </summary>
        public void Logout()
        {
            _currentUser = null;
        }

        /// <summary>
        /// Lấy tất cả người dùng
        /// </summary>
        public List<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        /// <summary>
        /// Tạo tài khoản mới
        /// </summary>
        public User? Register(string username, string password, string displayName, UserRole role = UserRole.Student)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            // Kiểm tra username đã tồn tại chưa
            var existingUser = _userRepository.GetUserByUsername(username);
            if (existingUser != null)
            {
                return null; // Username đã tồn tại
            }

            var users = _userRepository.GetAllUsers();
            int newId = users.Any() ? users.Max(u => u.Id) + 1 : 1;

            var newUser = new User
            {
                Id = newId,
                Username = username.Trim(),
                Password = password,
                DisplayName = string.IsNullOrWhiteSpace(displayName) ? username : displayName.Trim(),
                Role = role,
                CreatedDate = DateTime.Now
            };

            users.Add(newUser);

            if (_userRepository.SaveAllUsers(users))
            {
                return newUser;
            }

            return null;
        }
    }
}