using System.Text.Json;
using ASM.Entities.Models;

namespace ASM.Data.Repositories
{
    /// <summary>
    /// Repository quan ly nguoi dung - doc/ghi tu file users.json
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
        /// Lay tat ca nguoi dung
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
                Console.WriteLine($"Loi khi doc file users.json: {ex.Message}");
                return new List<User>();
            }
        }

        /// <summary>
        /// Luu tat ca nguoi dung xuong file
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
                Console.WriteLine($"Loi khi ghi file users.json: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Tim nguoi dung theo username
        /// </summary>
        public User? GetUserByUsername(string username)
        {
            var users = GetAllUsers();
            return users.FirstOrDefault(u => 
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Xac thuc nguoi dung
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
