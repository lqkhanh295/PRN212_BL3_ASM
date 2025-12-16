namespace ASM.Entities.Models
{
    /// <summary>
    /// Model ??i di?n cho m?t b? th? (Deck) ch?a nhi?u Flashcard
    /// </summary>
    public class Deck
    {
        /// <summary>
        /// ID duy nh?t c?a b? th?
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Tên b? th? (ví d?: "Ti?ng Anh TOEIC", "T? v?ng N5")
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Danh sách các th? flashcard trong b?
        /// </summary>
        public List<Flashcard> Flashcards { get; set; } = new List<Flashcard>();

        /// <summary>
        /// Ngày t?o b? th?
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// S? l??ng th? trong b? (thu?c tính tính toán)
        /// </summary>
        public int CardCount => Flashcards.Count;
    }
}