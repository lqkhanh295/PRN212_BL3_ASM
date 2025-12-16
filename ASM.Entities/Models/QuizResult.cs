namespace ASM.Entities.Models
{
    /// <summary>
    /// Model lưu trữ kết quả học tập của người dùng
    /// </summary>
    public class QuizResult
    {
        /// <summary>
        /// ID duy nhất của kết quả
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID người dùng
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// ID bộ thẻ đã học
        /// </summary>
        public int DeckId { get; set; }

        /// <summary>
        /// Tên bộ thẻ (để hiển thị)
        /// </summary>
        public string DeckName { get; set; } = string.Empty;

        /// <summary>
        /// Tổng số thẻ trong bài quiz
        /// </summary>
        public int TotalCards { get; set; }

        /// <summary>
        /// Số câu trả lời đúng
        /// </summary>
        public int CorrectAnswers { get; set; }

        /// <summary>
        /// Điểm phần trăm
        /// </summary>
        public double ScorePercent => TotalCards > 0 ? (double)CorrectAnswers / TotalCards * 100 : 0;

        /// <summary>
        /// Thời gian hoàn thành (giây)
        /// </summary>
        public int TimeSpentSeconds { get; set; }

        /// <summary>
        /// Ngày thực hiện quiz
        /// </summary>
        public DateTime CompletedDate { get; set; } = DateTime.Now;
    }
}