using System;
using System.Collections.Generic;
using System.IO;
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
            // Tìm thư mục gốc project dựa trên AppDomain.CurrentDomain.BaseDirectory
            // (ứng dụng chạy từ bin/... nên up lên vài cấp để về root project)
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectRoot = Path.GetFullPath(Path.Combine(baseDirectory, "..", "..", ".."));

            // File data.json ở thư mục gốc project
            _filePath = Path.Combine(projectRoot, "data.json");

            // Cấu hình JSON: indent để dễ đọc, cho phép tiếng Việt
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true, // Format JSON đẹp, dễ đọc
                PropertyNameCaseInsensitive = true, // Không phân biệt hoa thường khi đọc
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // Hỗ trợ tiếng Việt
            };
        }

        /// <summary>
        /// Đọc tất cả các Deck từ file JSON
        /// </summary>
        /// <returns>Danh sách Deck, trả về list rỗng nếu file chưa tồn tại</returns>
        public List<Deck> GetAllDecks()
        {
            try
            {
                // Kiểm tra file có tồn tại không
                if (!File.Exists(_filePath))
                {
                    // File chưa có -> trả về list rỗng
                    return new List<Deck>();
                }

                // Đọc nội dung file
                string jsonContent = File.ReadAllText(_filePath);

                // Nếu file rỗng -> trả về list rỗng
                if (string.IsNullOrWhiteSpace(jsonContent))
                {
                    return new List<Deck>();
                }

                // Chuyển đổi JSON thành List<Deck>
                var decks = JsonSerializer.Deserialize<List<Deck>>(jsonContent, _jsonOptions);

                // Trả về list deck hoặc list rỗng nếu null
                return decks ?? new List<Deck>();
            }
            catch (Exception ex)
            {
                // Ghi log lỗi (trong thực tế nên dùng logging framework)
                Console.WriteLine($"Lỗi khi đọc file JSON: {ex.Message}");
                Console.WriteLine($"Đường dẫn file: {_filePath}");
                return new List<Deck>();
            }
        }

        /// <summary>
        /// Lưu tất cả Deck xuống file JSON (ghi đè toàn bộ)
        /// </summary>
        /// <param name="decks">Danh sách Deck cần lưu</param>
        /// <returns>True nếu lưu thành công, False nếu có lỗi</returns>
        public bool SaveAllDecks(List<Deck> decks)
        {
            try
            {
                // Chuyển đổi List<Deck> thành chuỗi JSON
                string jsonContent = JsonSerializer.Serialize(decks, _jsonOptions);

                // Ghi đè xuống file
                File.WriteAllText(_filePath, jsonContent);

                Console.WriteLine($"Đã lưu dữ liệu vào: {_filePath}");
                return true;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                Console.WriteLine($"Lỗi khi ghi file JSON: {ex.Message}");
                Console.WriteLine($"Đường dẫn file: {_filePath}");
                return false;
            }
        }

        /// <summary>
        /// Lấy đường dẫn file data.json (để debug hoặc hiển thị cho user)
        /// </summary>
        public string GetDataFilePath() => _filePath;
    }
}