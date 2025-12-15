namespace ASM.Entities.Models
{
    /// <summary>
    /// Model ??i di?n cho m?t th? flashcard (th? h?c t? v?ng)
    /// </summary>
    public class Flashcard
    {
        /// <summary>
        /// ID duy nh?t c?a th?
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Thu?t ng?/t? v?ng c?n h?c (m?t tr??c th?)
        /// </summary>
        public string Term { get; set; } = string.Empty;

        /// <summary>
        /// ??nh ngh?a/ngh?a c?a t? (m?t sau th?)
        /// </summary>
        public string Definition { get; set; } = string.Empty;

        /// <summary>
        /// ?ánh d?u th? quan tr?ng ?? ôn t?p sau
        /// </summary>
        public bool IsBookmarked { get; set; }
    }
}
