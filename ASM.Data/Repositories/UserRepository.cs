using System.Text.Json;
using ASM.Entities.Models;

namespace ASM.Data.Repositories
{
    /// <summary>
    /// Repository quản lý người dùng - đọc/ghi từ file users.json
    /// </summary>
    public class UserRepository
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _jsonOptions;

        public UserRepository()
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users.json");
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };
        }

        /// <summary>
        /// Lấy tất cả người dùng
        /// </summary>
        public List<User> GetAllUsers()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    return new List<User>();
                }

                string jsonContent = File.ReadAllText(_filePath);

                if (string.IsNullOrWhiteSpace(jsonContent))
                {
                    return new List<User>();
                }

                var users = JsonSerializer.Deserialize<List<User>>(jsonContent, _jsonOptions);
                return users ?? new List<User>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi đọc file users.json: {ex.Message}");
                return new List<User>();
            }
        }

        /// <summary>
        /// Lưu tất cả người dùng xuống file
        /// </summary>
        public bool SaveAllUsers(List<User> users)
        {
            try
            {
                string jsonContent = JsonSerializer.Serialize(users, _jsonOptions);
                File.WriteAllText(_filePath, jsonContent);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi ghi file users.json: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Tìm người dùng theo username
        /// </summary>
        public User? GetUserByUsername(string username)
        {
            var users = GetAllUsers();
            return users.FirstOrDefault(u =>
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Xác thực người dùng
        /// </summary>
        public User? Authenticate(string username, string password)
        {
            var user = GetUserByUsername(username);
            if (user != null && user.Password == password)
            {
                return user;
            }
            return null;
        }
    }
}