using ASM.Data.Repositories;
using ASM.Entities.Models;

namespace ASM.Bussiness.Services
{
    /// <summary>
    /// Service xử lý nghiệp vụ liên quan đến Deck và Flashcard
    /// Đây là tầng Business Logic Layer (BLL) - xử lý logic nghiệp vụ
    /// </summary>
    public class DeckService
    {
        // Repository để truy cập dữ liệu (tầng DAL)
        private readonly JsonRepository _repository;

        /// <summary>
        /// Constructor - khởi tạo service với repository
        /// </summary>
        public DeckService()
        {
            _repository = new JsonRepository();
        }

        #region Deck Operations (Thao tác với bộ thẻ)

        /// <summary>
        /// Lấy tất cả các bộ thẻ
        /// </summary>
        /// <returns>Danh sách tất cả Deck</returns>
        public List<Deck> GetAllDecks()
        {
            return _repository.GetAllDecks();
        }

        /// <summary>
        /// Lấy một bộ thẻ theo ID
        /// </summary>
        /// <param name="deckId">ID của bộ thẻ cần tìm</param>
        /// <returns>Deck nếu tìm thấy, null nếu không</returns>
        public Deck? GetDeckById(int deckId)
        {
            var decks = _repository.GetAllDecks();
            return decks.FirstOrDefault(d => d.Id == deckId);
        }

        /// <summary>
        /// Tạo một bộ thẻ mới
        /// </summary>
        /// <param name="name">Tên bộ thẻ</param>
        /// <returns>Deck vừa tạo, null nếu thất bại</returns>
        public Deck? CreateDeck(string name)
        {
            // Validate: tên không được rỗng
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            // Lấy danh sách deck hiện tại từ DAL
            var decks = _repository.GetAllDecks();

            // Tạo ID mới (lấy max ID + 1, hoặc 1 nếu chưa có deck nào)
            int newId = decks.Any() ? decks.Max(d => d.Id) + 1 : 1;

            // Tạo deck mới
            var newDeck = new Deck
            {
                Id = newId,
                Name = name.Trim(),
                CreatedDate = DateTime.Now,
                Flashcards = new List<Flashcard>()
            };

            // Thêm vào danh sách
            decks.Add(newDeck);

            // Lưu xuống DAL
            if (_repository.SaveAllDecks(decks))
            {
                return newDeck;
            }

            return null;
        }

        /// <summary>
        /// Cập nhật tên bộ thẻ
        /// </summary>
        /// <param name="deckId">ID bộ thẻ cần cập nhật</param>
        /// <param name="newName">Tên mới</param>
        /// <returns>True nếu thành công</returns>
        public bool UpdateDeck(int deckId, string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                return false;
            }

            var decks = _repository.GetAllDecks();
            var deck = decks.FirstOrDefault(d => d.Id == deckId);

            if (deck == null)
            {
                return false;
            }

            deck.Name = newName.Trim();
            return _repository.SaveAllDecks(decks);
        }

        /// <summary>
        /// Xóa một bộ thẻ
        /// </summary>
        /// <param name="deckId">ID bộ thẻ cần xóa</param>
        /// <returns>True nếu xóa thành công</returns>
        public bool DeleteDeck(int deckId)
        {
            var decks = _repository.GetAllDecks();
            var deck = decks.FirstOrDefault(d => d.Id == deckId);

            if (deck == null)
            {
                return false;
            }

            decks.Remove(deck);
            return _repository.SaveAllDecks(decks);
        }

        #endregion

        #region Flashcard Operations (Thao tác với thẻ)

        /// <summary>
        /// Thêm một thẻ mới vào bộ thẻ
        /// </summary>
        /// <param name="deckId">ID bộ thẻ</param>
        /// <param name="card">Thẻ cần thêm</param>
        /// <returns>True nếu thêm thành công</returns>
        public bool AddCardToDeck(int deckId, Flashcard card)
        {
            // Validate input
            if (card == null || string.IsNullOrWhiteSpace(card.Term))
            {
                return false;
            }

            // Lấy danh sách deck từ DAL
            var decks = _repository.GetAllDecks();

            // Tìm deck cần thêm thẻ
            var deck = decks.FirstOrDefault(d => d.Id == deckId);
            if (deck == null)
            {
                return false;
            }

            // Tạo ID mới cho thẻ
            int newCardId = deck.Flashcards.Any()
                ? deck.Flashcards.Max(f => f.Id) + 1
                : 1;

            card.Id = newCardId;

            // Thêm thẻ vào deck
            deck.Flashcards.Add(card);

            // Lưu xuống DAL
            return _repository.SaveAllDecks(decks);
        }

        /// <summary>
        /// Cập nhật một thẻ flashcard
        /// </summary>
        /// <param name="deckId">ID bộ thẻ chứa thẻ</param>
        /// <param name="card">Thẻ với thông tin mới</param>
        /// <returns>True nếu cập nhật thành công</returns>
        public bool UpdateCard(int deckId, Flashcard card)
        {
            if (card == null)
            {
                return false;
            }

            var decks = _repository.GetAllDecks();
            var deck = decks.FirstOrDefault(d => d.Id == deckId);

            if (deck == null)
            {
                return false;
            }

            var existingCard = deck.Flashcards.FirstOrDefault(f => f.Id == card.Id);
            if (existingCard == null)
            {
                return false;
            }

            // Cập nhật thông tin thẻ
            existingCard.Term = card.Term;
            existingCard.Definition = card.Definition;
            existingCard.IsBookmarked = card.IsBookmarked;

            return _repository.SaveAllDecks(decks);
        }

        /// <summary>
        /// Xóa một thẻ khỏi bộ thẻ
        /// </summary>
        /// <param name="deckId">ID bộ thẻ</param>
        /// <param name="cardId">ID thẻ cần xóa</param>
        /// <returns>True nếu xóa thành công</returns>
        public bool DeleteCard(int deckId, int cardId)
        {
            var decks = _repository.GetAllDecks();
            var deck = decks.FirstOrDefault(d => d.Id == deckId);

            if (deck == null)
            {
                return false;
            }

            var card = deck.Flashcards.FirstOrDefault(f => f.Id == cardId);
            if (card == null)
            {
                return false;
            }

            deck.Flashcards.Remove(card);
            return _repository.SaveAllDecks(decks);
        }

        /// <summary>
        /// Toggle bookmark cho một thẻ
        /// </summary>
        /// <param name="deckId">ID bộ thẻ</param>
        /// <param name="cardId">ID thẻ</param>
        /// <returns>True nếu thành công</returns>
        public bool ToggleBookmark(int deckId, int cardId)
        {
            var decks = _repository.GetAllDecks();
            var deck = decks.FirstOrDefault(d => d.Id == deckId);

            if (deck == null)
            {
                return false;
            }

            var card = deck.Flashcards.FirstOrDefault(f => f.Id == cardId);
            if (card == null)
            {
                return false;
            }

            // Đảo trạng thái bookmark
            card.IsBookmarked = !card.IsBookmarked;
            return _repository.SaveAllDecks(decks);
        }

        #endregion
    }
}