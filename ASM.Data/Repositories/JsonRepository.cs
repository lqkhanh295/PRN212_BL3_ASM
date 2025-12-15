using System.Text.Json;
using ASM.Entities.Models;

namespace ASM.Data.Repositories
{
    /// <summary>
    /// Repository ch?u trách nhi?m ??c/ghi d? li?u t? file JSON
    /// ?ây là t?ng Data Access Layer (DAL) - ch? làm vi?c v?i d? li?u
    /// </summary>
    public class JsonRepository
    {
        // ???ng d?n file JSON l?u tr? d? li?u
        private readonly string _filePath;

        // C?u hình JSON ?? format ??p khi ghi file
        private readonly JsonSerializerOptions _jsonOptions;

        /// <summary>
        /// Constructor - kh?i t?o repository v?i ???ng d?n file m?c ??nh
        /// </summary>
        public JsonRepository()
        {
            // File data.json ???c l?u cùng th? m?c v?i ?ng d?ng
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.json");

            // C?u hình JSON: indent ?? d? ??c, cho phép ti?ng Vi?t
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true, // Format JSON ??p, d? ??c
                PropertyNameCaseInsensitive = true, // Không phân bi?t hoa th??ng khi ??c
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // H? tr? ti?ng Vi?t
            };
        }

        /// <summary>
        /// ??c t?t c? các Deck t? file JSON
        /// </summary>
        /// <returns>Danh sách Deck, tr? v? list r?ng n?u file ch?a t?n t?i</returns>
        public List<Deck> GetAllDecks()
        {
            try
            {
                // Ki?m tra file có t?n t?i không
                if (!File.Exists(_filePath))
                {
                    // File ch?a có -> tr? v? list r?ng
                    return new List<Deck>();
                }

                // ??c n?i dung file
                string jsonContent = File.ReadAllText(_filePath);

                // N?u file r?ng -> tr? v? list r?ng
                if (string.IsNullOrWhiteSpace(jsonContent))
                {
                    return new List<Deck>();
                }

                // Chuy?n ??i JSON thành List<Deck>
                var decks = JsonSerializer.Deserialize<List<Deck>>(jsonContent, _jsonOptions);

                // Tr? v? list deck ho?c list r?ng n?u null
                return decks ?? new List<Deck>();
            }
            catch (Exception ex)
            {
                // Ghi log l?i (trong th?c t? nên dùng logging framework)
                Console.WriteLine($"L?i khi ??c file JSON: {ex.Message}");
                return new List<Deck>();
            }
        }

        /// <summary>
        /// L?u t?t c? Deck xu?ng file JSON (ghi ?è toàn b?)
        /// </summary>
        /// <param name="decks">Danh sách Deck c?n l?u</param>
        /// <returns>True n?u l?u thành công, False n?u có l?i</returns>
        public bool SaveAllDecks(List<Deck> decks)
        {
            try
            {
                // Chuy?n ??i List<Deck> thành chu?i JSON
                string jsonContent = JsonSerializer.Serialize(decks, _jsonOptions);

                // Ghi ?è xu?ng file
                File.WriteAllText(_filePath, jsonContent);

                return true;
            }
            catch (Exception ex)
            {
                // Ghi log l?i
                Console.WriteLine($"L?i khi ghi file JSON: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// L?y ???ng d?n file data.json (?? debug ho?c hi?n th? cho user)
        /// </summary>
        public string GetDataFilePath() => _filePath;
    }
}
