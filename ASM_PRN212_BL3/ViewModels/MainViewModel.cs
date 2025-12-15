using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ASM.Bussiness.Services;
using ASM.Entities.Models;

namespace ASM_PRN212_BL3.ViewModels
{
    /// <summary>
    /// ViewModel chính c?a ?ng d?ng
    /// Qu?n lý danh sách Deck và các thao tác liên quan
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        // Service t? t?ng BLL
        private readonly DeckService _deckService;

        #region Properties (Thu?c tính binding v?i View)

        // Danh sách các b? th? - ObservableCollection t? ??ng thông báo khi thêm/xóa item
        private ObservableCollection<Deck> _decks = new();
        public ObservableCollection<Deck> Decks
        {
            get => _decks;
            set => SetProperty(ref _decks, value);
        }

        // B? th? ?ang ???c ch?n
        private Deck? _selectedDeck;
        public Deck? SelectedDeck
        {
            get => _selectedDeck;
            set
            {
                if (SetProperty(ref _selectedDeck, value))
                {
                    // Khi ch?n deck khác, load danh sách th? c?a deck ?ó
                    LoadFlashcards();
                }
            }
        }

        // Danh sách th? c?a deck ?ang ch?n
        private ObservableCollection<Flashcard> _flashcards = new();
        public ObservableCollection<Flashcard> Flashcards
        {
            get => _flashcards;
            set => SetProperty(ref _flashcards, value);
        }

        // Th? ?ang ???c ch?n
        private Flashcard? _selectedFlashcard;
        public Flashcard? SelectedFlashcard
        {
            get => _selectedFlashcard;
            set => SetProperty(ref _selectedFlashcard, value);
        }

        // Tên deck m?i (?? t?o deck)
        private string _newDeckName = string.Empty;
        public string NewDeckName
        {
            get => _newDeckName;
            set => SetProperty(ref _newDeckName, value);
        }

        // Thu?t ng? m?i (?? t?o flashcard)
        private string _newTerm = string.Empty;
        public string NewTerm
        {
            get => _newTerm;
            set => SetProperty(ref _newTerm, value);
        }

        // ??nh ngh?a m?i (?? t?o flashcard)
        private string _newDefinition = string.Empty;
        public string NewDefinition
        {
            get => _newDefinition;
            set => SetProperty(ref _newDefinition, value);
        }

        // Thông báo tr?ng thái cho ng??i dùng
        private string _statusMessage = string.Empty;
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        #endregion

        #region Commands (Các l?nh binding v?i View)

        // Command ?? load danh sách deck
        public ICommand LoadDecksCommand { get; }

        // Command ?? t?o deck m?i
        public ICommand CreateDeckCommand { get; }

        // Command ?? xóa deck ?ang ch?n
        public ICommand DeleteDeckCommand { get; }

        // Command ?? thêm th? m?i vào deck
        public ICommand AddFlashcardCommand { get; }

        // Command ?? xóa th? ?ang ch?n
        public ICommand DeleteFlashcardCommand { get; }

        // Command ?? toggle bookmark
        public ICommand ToggleBookmarkCommand { get; }

        #endregion

        /// <summary>
        /// Constructor - kh?i t?o ViewModel
        /// </summary>
        public MainViewModel()
        {
            // Kh?i t?o service t? BLL
            _deckService = new DeckService();

            // Kh?i t?o các Command
            LoadDecksCommand = new RelayCommand(_ => LoadDecks());
            CreateDeckCommand = new RelayCommand(_ => CreateDeck(), _ => CanCreateDeck());
            DeleteDeckCommand = new RelayCommand(_ => DeleteDeck(), _ => SelectedDeck != null);
            AddFlashcardCommand = new RelayCommand(_ => AddFlashcard(), _ => CanAddFlashcard());
            DeleteFlashcardCommand = new RelayCommand(_ => DeleteFlashcard(), _ => SelectedFlashcard != null);
            ToggleBookmarkCommand = new RelayCommand<Flashcard>(ToggleBookmark);

            // T? ??ng load d? li?u khi kh?i t?o
            LoadDecks();
        }

        #region Command Methods (Các ph??ng th?c x? lý)

        /// <summary>
        /// Load t?t c? deck t? BLL
        /// </summary>
        private void LoadDecks()
        {
            try
            {
                var decks = _deckService.GetAllDecks();
                Decks = new ObservableCollection<Deck>(decks);
                StatusMessage = $"?ã t?i {decks.Count} b? th?";
            }
            catch (Exception ex)
            {
                StatusMessage = $"L?i: {ex.Message}";
            }
        }

        /// <summary>
        /// Load danh sách flashcard c?a deck ?ang ch?n
        /// </summary>
        private void LoadFlashcards()
        {
            if (SelectedDeck != null)
            {
                Flashcards = new ObservableCollection<Flashcard>(SelectedDeck.Flashcards);
                StatusMessage = $"B? th? '{SelectedDeck.Name}' có {SelectedDeck.CardCount} th?";
            }
            else
            {
                Flashcards.Clear();
            }
        }

        /// <summary>
        /// Ki?m tra có th? t?o deck m?i không
        /// </summary>
        private bool CanCreateDeck()
        {
            return !string.IsNullOrWhiteSpace(NewDeckName);
        }

        /// <summary>
        /// T?o deck m?i
        /// </summary>
        private void CreateDeck()
        {
            var newDeck = _deckService.CreateDeck(NewDeckName);
            if (newDeck != null)
            {
                Decks.Add(newDeck);
                NewDeckName = string.Empty; // Clear input
                StatusMessage = $"?ã t?o b? th? '{newDeck.Name}'";
            }
            else
            {
                StatusMessage = "Không th? t?o b? th?";
            }
        }

        /// <summary>
        /// Xóa deck ?ang ch?n
        /// </summary>
        private void DeleteDeck()
        {
            if (SelectedDeck == null) return;

            // Hi?n h?p tho?i xác nh?n
            var result = MessageBox.Show(
                $"B?n có ch?c mu?n xóa b? th? '{SelectedDeck.Name}'?",
                "Xác nh?n xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (_deckService.DeleteDeck(SelectedDeck.Id))
                {
                    var deletedName = SelectedDeck.Name;
                    Decks.Remove(SelectedDeck);
                    SelectedDeck = null;
                    StatusMessage = $"?ã xóa b? th? '{deletedName}'";
                }
            }
        }

        /// <summary>
        /// Ki?m tra có th? thêm flashcard không
        /// </summary>
        private bool CanAddFlashcard()
        {
            return SelectedDeck != null && !string.IsNullOrWhiteSpace(NewTerm);
        }

        /// <summary>
        /// Thêm flashcard m?i vào deck ?ang ch?n
        /// </summary>
        private void AddFlashcard()
        {
            if (SelectedDeck == null) return;

            var newCard = new Flashcard
            {
                Term = NewTerm.Trim(),
                Definition = NewDefinition.Trim(),
                IsBookmarked = false
            };

            if (_deckService.AddCardToDeck(SelectedDeck.Id, newCard))
            {
                Flashcards.Add(newCard);
                
                // Clear input
                NewTerm = string.Empty;
                NewDefinition = string.Empty;
                
                StatusMessage = $"?ã thêm th? '{newCard.Term}'";
            }
            else
            {
                StatusMessage = "Không th? thêm th?";
            }
        }

        /// <summary>
        /// Xóa flashcard ?ang ch?n
        /// </summary>
        private void DeleteFlashcard()
        {
            if (SelectedDeck == null || SelectedFlashcard == null) return;

            if (_deckService.DeleteCard(SelectedDeck.Id, SelectedFlashcard.Id))
            {
                var deletedTerm = SelectedFlashcard.Term;
                Flashcards.Remove(SelectedFlashcard);
                SelectedFlashcard = null;
                StatusMessage = $"?ã xóa th? '{deletedTerm}'";
            }
        }

        /// <summary>
        /// Toggle bookmark cho m?t th?
        /// </summary>
        private void ToggleBookmark(Flashcard? card)
        {
            if (SelectedDeck == null || card == null) return;

            if (_deckService.ToggleBookmark(SelectedDeck.Id, card.Id))
            {
                card.IsBookmarked = !card.IsBookmarked;
                StatusMessage = card.IsBookmarked 
                    ? $"?ã ?ánh d?u '{card.Term}'" 
                    : $"?ã b? ?ánh d?u '{card.Term}'";
            }
        }

        #endregion
    }
}
