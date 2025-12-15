using ASM.Data.Repositories;
using ASM.Entities.Models;

namespace ASM.Bussiness.Services
{
    /// <summary>
    /// Service xu ly nghiep vu lien quan den nguoi dung
    /// </summary>
    public class UserService
    {
        private readonly UserRepository _userRepository;

        // Nguoi dung hien tai dang dang nhap (static de dung chung)
        private static User? _currentUser;

        public UserService()
        {
            _userRepository = new UserRepository();
        }

        /// <summary>
        /// Lay nguoi dung hien tai
        /// </summary>
        public static User? CurrentUser => _currentUser;

        /// <summary>
        /// Kiem tra nguoi dung hien tai co phai Admin khong
        /// </summary>
        public static bool IsAdmin => _currentUser?.Role == UserRole.Admin;

        /// <summary>
        /// Kiem tra nguoi dung hien tai co phai Student khong
        /// </summary>
        public static bool IsStudent => _currentUser?.Role == UserRole.Student;

        /// <summary>
        /// Dang nhap vao he thong
        /// </summary>
        /// <param name="username">Ten dang nhap</param>
        /// <param name="password">Mat khau</param>
        /// <returns>User neu thanh cong, null neu that bai</returns>
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
        /// Dang xuat khoi he thong
        /// </summary>
        public void Logout()
        {
            _currentUser = null;
        }

        /// <summary>
        /// Lay tat ca nguoi dung
        /// </summary>
        public List<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        /// <summary>
        /// Tao tai khoan moi
        /// </summary>
        public User? Register(string username, string password, string displayName, UserRole role = UserRole.Student)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            // Kiem tra username da ton tai chua
            var existingUser = _userRepository.GetUserByUsername(username);
            if (existingUser != null)
            {
                return null; // Username da ton tai
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
