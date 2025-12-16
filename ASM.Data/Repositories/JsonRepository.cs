using System.Text.Json;
using ASM.Entities.Models;

namespace ASM.Data.Repositories
{
    /// <summary>
    /// Repository chịu trách nhiệm đọc/ghi dữ liệu từ file JSON
    /// Đây là tầng Data Access Layer (DAL) - chỉ làm việc với dữ liệu
    /// </summary>
    public class JsonRepository
    {
        // Đường dẫn file JSON lưu trữ dữ liệu
        private readonly string _filePath;

        // Cấu hình JSON để format đẹp khi ghi file
        private readonly JsonSerializerOptions _jsonOptions;

        /// <summary>
        /// Constructor - khởi tạo repository với đường dẫn file mặc định
        /// </summary>
        public JsonRepository()
        {
<<<<<<< HEAD
<<<<<<< HEAD
            // File data.json được lưu cùng thư mục với ứng dụng
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.json");

            // Cấu hình JSON: indent để dễ đọc, cho phép tiếng Việt
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true, // Format JSON đẹp, dễ đọc
                PropertyNameCaseInsensitive = true, // Không phân biệt hoa thường khi đọc
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // Hỗ trợ tiếng Việt
=======
            // T�m th? m?c g?c project (thay v� bin/Debug)
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            
            // Di chuy?n l�n 3 c?p: bin\Debug\net9.0-windows -> project root
            string projectRoot = Path.GetFullPath(Path.Combine(baseDirectory, "..", "..", ".."));
            
            // File data.json ? th? m?c g?c project
            _filePath = Path.Combine(projectRoot, "data.json");

=======
            // T�m th? m?c g?c project (thay v� bin/Debug)
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            
            // Di chuy?n l�n 3 c?p: bin\Debug\net9.0-windows -> project root
            string projectRoot = Path.GetFullPath(Path.Combine(baseDirectory, "..", "..", ".."));
            
            // File data.json ? th? m?c g?c project
            _filePath = Path.Combine(projectRoot, "data.json");

>>>>>>> 5b835f85684f5e91133de1435d46ffa8ac8bc8b7
            // C?u h?nh JSON: indent ?? d? ??c, cho ph?p ti?ng Vi?t
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true, // Format JSON ??p, d? ??c
                PropertyNameCaseInsensitive = true, // Kh?ng ph?n bi?t hoa th??ng khi ??c
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // H? tr? ti?ng Vi?t
>>>>>>> 5b835f85684f5e91133de1435d46ffa8ac8bc8b7
            };
        }

        /// <summary>
<<<<<<< HEAD
<<<<<<< HEAD
        /// Đọc tất cả các Deck từ file JSON
        /// </summary>
        /// <returns>Danh sách Deck, trả về list rỗng nếu file chưa tồn tại</returns>
=======
        /// ??c t?t c? c?c Deck t? file JSON
        /// </summary>
        /// <returns>Danh s?ch Deck, tr? v? list r?ng n?u file ch?a t?n t?i</returns>
>>>>>>> 5b835f85684f5e91133de1435d46ffa8ac8bc8b7
=======
        /// ??c t?t c? c?c Deck t? file JSON
        /// </summary>
        /// <returns>Danh s?ch Deck, tr? v? list r?ng n?u file ch?a t?n t?i</returns>
>>>>>>> 5b835f85684f5e91133de1435d46ffa8ac8bc8b7
        public List<Deck> GetAllDecks()
        {
            try
            {
<<<<<<< HEAD
<<<<<<< HEAD
                // Kiểm tra file có tồn tại không
                if (!File.Exists(_filePath))
                {
                    // File chưa có -> trả về list rỗng
=======
                // Ki?m tra file c? t?n t?i kh?ng
                if (!File.Exists(_filePath))
                {
                    // File ch?a c? -> tr? v? list r?ng
>>>>>>> 5b835f85684f5e91133de1435d46ffa8ac8bc8b7
=======
                // Ki?m tra file c? t?n t?i kh?ng
                if (!File.Exists(_filePath))
                {
                    // File ch?a c? -> tr? v? list r?ng
>>>>>>> 5b835f85684f5e91133de1435d46ffa8ac8bc8b7
                    return new List<Deck>();
                }

                // Đọc nội dung file
                string jsonContent = File.ReadAllText(_filePath);

                // Nếu file rỗng -> trả về list rỗng
                if (string.IsNullOrWhiteSpace(jsonContent))
                {
                    return new List<Deck>();
                }

<<<<<<< HEAD
<<<<<<< HEAD
                // Chuyển đổi JSON thành List<Deck>
=======
                // Chuy?n ??i JSON th?nh List<Deck>
>>>>>>> 5b835f85684f5e91133de1435d46ffa8ac8bc8b7
=======
                // Chuy?n ??i JSON th?nh List<Deck>
>>>>>>> 5b835f85684f5e91133de1435d46ffa8ac8bc8b7
                var decks = JsonSerializer.Deserialize<List<Deck>>(jsonContent, _jsonOptions);

                // Trả về list deck hoặc list rỗng nếu null
                return decks ?? new List<Deck>();
            }
            catch (Exception ex)
            {
<<<<<<< HEAD
<<<<<<< HEAD
                // Ghi log lỗi (trong thực tế nên dùng logging framework)
                Console.WriteLine($"Lỗi khi đọc file JSON: {ex.Message}");
=======
                // Ghi log l?i (trong th?c t? n?n d?ng logging framework)
                Console.WriteLine($"L?i khi ??c file JSON: {ex.Message}");
                Console.WriteLine($"???ng d?n file: {_filePath}");
>>>>>>> 5b835f85684f5e91133de1435d46ffa8ac8bc8b7
=======
                // Ghi log l?i (trong th?c t? n?n d?ng logging framework)
                Console.WriteLine($"L?i khi ??c file JSON: {ex.Message}");
                Console.WriteLine($"???ng d?n file: {_filePath}");
>>>>>>> 5b835f85684f5e91133de1435d46ffa8ac8bc8b7
                return new List<Deck>();
            }
        }

        /// <summary>
<<<<<<< HEAD
<<<<<<< HEAD
        /// Lưu tất cả Deck xuống file JSON (ghi đè toàn bộ)
        /// </summary>
        /// <param name="decks">Danh sách Deck cần lưu</param>
        /// <returns>True nếu lưu thành công, False nếu có lỗi</returns>
=======
        /// L?u t?t c? Deck xu?ng file JSON (ghi ?? to�n b?)
        /// </summary>
        /// <param name="decks">Danh s?ch Deck c?n l?u</param>
        /// <returns>True n?u l?u th?nh c?ng, False n?u c? l?i</returns>
>>>>>>> 5b835f85684f5e91133de1435d46ffa8ac8bc8b7
=======
        /// L?u t?t c? Deck xu?ng file JSON (ghi ?? to�n b?)
        /// </summary>
        /// <param name="decks">Danh s?ch Deck c?n l?u</param>
        /// <returns>True n?u l?u th?nh c?ng, False n?u c? l?i</returns>
>>>>>>> 5b835f85684f5e91133de1435d46ffa8ac8bc8b7
        public bool SaveAllDecks(List<Deck> decks)
        {
            try
            {
<<<<<<< HEAD
<<<<<<< HEAD
                // Chuyển đổi List<Deck> thành chuỗi JSON
                string jsonContent = JsonSerializer.Serialize(decks, _jsonOptions);

                // Ghi đè xuống file
=======
                // Chuy?n ??i List<Deck> th?nh chu?i JSON
                string jsonContent = JsonSerializer.Serialize(decks, _jsonOptions);

                // Ghi ?? xu?ng file
>>>>>>> 5b835f85684f5e91133de1435d46ffa8ac8bc8b7
=======
                // Chuy?n ??i List<Deck> th?nh chu?i JSON
                string jsonContent = JsonSerializer.Serialize(decks, _jsonOptions);

                // Ghi ?? xu?ng file
>>>>>>> 5b835f85684f5e91133de1435d46ffa8ac8bc8b7
                File.WriteAllText(_filePath, jsonContent);

                Console.WriteLine($"?? l?u d? li?u v?o: {_filePath}");
                return true;
            }
            catch (Exception ex)
            {
<<<<<<< HEAD
                // Ghi log lỗi
                Console.WriteLine($"Lỗi khi ghi file JSON: {ex.Message}");
=======
                // Ghi log l?i
                Console.WriteLine($"L?i khi ghi file JSON: {ex.Message}");
                Console.WriteLine($"???ng d?n file: {_filePath}");
<<<<<<< HEAD
>>>>>>> 5b835f85684f5e91133de1435d46ffa8ac8bc8b7
=======
>>>>>>> 5b835f85684f5e91133de1435d46ffa8ac8bc8b7
                return false;
            }
        }

        /// <summary>
        /// Lấy đường dẫn file data.json (để debug hoặc hiển thị cho user)
        /// </summary>
        public string GetDataFilePath() => _filePath;
    }
}