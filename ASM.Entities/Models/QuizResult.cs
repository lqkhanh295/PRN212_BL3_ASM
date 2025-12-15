namespace ASM.Entities.Models
{
    /// <summary>
    /// Model l?u tr? k?t qu? h?c t?p c?a ng??i dùng
    /// </summary>
    public class QuizResult
    {
        /// <summary>
        /// ID duy nh?t c?a k?t qu?
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID ng??i dùng
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// ID b? th? ?ã h?c
        /// </summary>
        public int DeckId { get; set; }

        /// <summary>
        /// Tên b? th? (?? hi?n th?)
        /// </summary>
        public string DeckName { get; set; } = string.Empty;

        /// <summary>
        /// T?ng s? th? trong bài quiz
        /// </summary>
        public int TotalCards { get; set; }

        /// <summary>
        /// S? câu tr? l?i ?úng
        /// </summary>
        public int CorrectAnswers { get; set; }

        /// <summary>
        /// ?i?m ph?n tr?m
        /// </summary>
        public double ScorePercent => TotalCards > 0 ? (double)CorrectAnswers / TotalCards * 100 : 0;

        /// <summary>
        /// Th?i gian hoàn thành (giây)
        /// </summary>
        public int TimeSpentSeconds { get; set; }

        /// <summary>
        /// Ngày th?c hi?n quiz
        /// </summary>
        public DateTime CompletedDate { get; set; } = DateTime.Now;
    }
}
