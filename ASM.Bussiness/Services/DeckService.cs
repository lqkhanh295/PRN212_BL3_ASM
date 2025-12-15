using ASM.Data.Repositories;
using ASM.Entities.Models;

namespace ASM.Bussiness.Services
{
    /// <summary>
    /// Service x? lý nghi?p v? liên quan ??n Deck và Flashcard
    /// ?ây là t?ng Business Logic Layer (BLL) - x? lý logic nghi?p v?
    /// </summary>
    public class DeckService
    {
        // Repository ?? truy c?p d? li?u (t?ng DAL)
        private readonly JsonRepository _repository;

        /// <summary>
        /// Constructor - kh?i t?o service v?i repository
        /// </summary>
        public DeckService()
        {
            _repository = new JsonRepository();
        }

        #region Deck Operations (Thao tác v?i b? th?)

        /// <summary>
        /// L?y t?t c? các b? th?
        /// </summary>
        /// <returns>Danh sách t?t c? Deck</returns>
        public List<Deck> GetAllDecks()
        {
            return _repository.GetAllDecks();
        }

        /// <summary>
        /// L?y m?t b? th? theo ID
        /// </summary>
        /// <param name="deckId">ID c?a b? th? c?n tìm</param>
        /// <returns>Deck n?u tìm th?y, null n?u không</returns>
        public Deck? GetDeckById(int deckId)
        {
            var decks = _repository.GetAllDecks();
            return decks.FirstOrDefault(d => d.Id == deckId);
        }

        /// <summary>
        /// T?o m?t b? th? m?i
        /// </summary>
        /// <param name="name">Tên b? th?</param>
        /// <returns>Deck v?a t?o, null n?u th?t b?i</returns>
        public Deck? CreateDeck(string name)
        {
            // Validate: tên không ???c r?ng
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            // L?y danh sách deck hi?n t?i t? DAL
            var decks = _repository.GetAllDecks();

            // T?o ID m?i (l?y max ID + 1, ho?c 1 n?u ch?a có deck nào)
            int newId = decks.Any() ? decks.Max(d => d.Id) + 1 : 1;

            // T?o deck m?i
            var newDeck = new Deck
            {
                Id = newId,
                Name = name.Trim(),
                CreatedDate = DateTime.Now,
                Flashcards = new List<Flashcard>()
            };

            // Thêm vào danh sách
            decks.Add(newDeck);

            // L?u xu?ng DAL
            if (_repository.SaveAllDecks(decks))
            {
                return newDeck;
            }

            return null;
        }

        /// <summary>
        /// C?p nh?t tên b? th?
        /// </summary>
        /// <param name="deckId">ID b? th? c?n c?p nh?t</param>
        /// <param name="newName">Tên m?i</param>
        /// <returns>True n?u thành công</returns>
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
        /// Xóa m?t b? th?
        /// </summary>
        /// <param name="deckId">ID b? th? c?n xóa</param>
        /// <returns>True n?u xóa thành công</returns>
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

        #region Flashcard Operations (Thao tác v?i th?)

        /// <summary>
        /// Thêm m?t th? m?i vào b? th?
        /// </summary>
        /// <param name="deckId">ID b? th?</param>
        /// <param name="card">Th? c?n thêm</param>
        /// <returns>True n?u thêm thành công</returns>
        public bool AddCardToDeck(int deckId, Flashcard card)
        {
            // Validate input
            if (card == null || string.IsNullOrWhiteSpace(card.Term))
            {
                return false;
            }

            // L?y danh sách deck t? DAL
            var decks = _repository.GetAllDecks();

            // Tìm deck c?n thêm th?
            var deck = decks.FirstOrDefault(d => d.Id == deckId);
            if (deck == null)
            {
                return false;
            }

            // T?o ID m?i cho th?
            int newCardId = deck.Flashcards.Any() 
                ? deck.Flashcards.Max(f => f.Id) + 1 
                : 1;

            card.Id = newCardId;

            // Thêm th? vào deck
            deck.Flashcards.Add(card);

            // L?u xu?ng DAL
            return _repository.SaveAllDecks(decks);
        }

        /// <summary>
        /// C?p nh?t m?t th? flashcard
        /// </summary>
        /// <param name="deckId">ID b? th? ch?a th?</param>
        /// <param name="card">Th? v?i thông tin m?i</param>
        /// <returns>True n?u c?p nh?t thành công</returns>
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

            // C?p nh?t thông tin th?
            existingCard.Term = card.Term;
            existingCard.Definition = card.Definition;
            existingCard.IsBookmarked = card.IsBookmarked;

            return _repository.SaveAllDecks(decks);
        }

        /// <summary>
        /// Xóa m?t th? kh?i b? th?
        /// </summary>
        /// <param name="deckId">ID b? th?</param>
        /// <param name="cardId">ID th? c?n xóa</param>
        /// <returns>True n?u xóa thành công</returns>
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
        /// Toggle bookmark cho m?t th?
        /// </summary>
        /// <param name="deckId">ID b? th?</param>
        /// <param name="cardId">ID th?</param>
        /// <returns>True n?u thành công</returns>
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

            // ??o tr?ng thái bookmark
            card.IsBookmarked = !card.IsBookmarked;
            return _repository.SaveAllDecks(decks);
        }

        #endregion
    }
}
