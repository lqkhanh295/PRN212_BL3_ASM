using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ASM.Bussiness.Services;
using ASM.Entities.Models;

namespace ASM_PRN212_BL3.ViewModels
{
    /// <summary>
    /// ViewModel chính của ứng dụng
    /// Quản lý danh sách Deck và các thao tác liên quan
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        // Service từ tầng BLL
        private readonly DeckService _deckService;

        #region Properties (Thuộc tính binding với View)

        // Danh sách các bộ thẻ - ObservableCollection tự động thông báo khi thêm/xóa item
        private ObservableCollection<Deck> _decks = new();
        public ObservableCollection<Deck> Decks
        {
            get => _decks;
            set => SetProperty(ref _decks, value);
        }

        // Bộ thẻ đang được chọn
        private Deck? _selectedDeck;
        public Deck? SelectedDeck
        {
            get => _selectedDeck;
            set
            {
                if (SetProperty(ref _selectedDeck, value))
                {
                    // Khi chọn deck khác, load danh sách thẻ của deck đó
                    LoadFlashcards();
                }
            }
        }

        // Danh sách thẻ của deck đang chọn
        private ObservableCollection<Flashcard> _flashcards = new();
        public ObservableCollection<Flashcard> Flashcards
        {
            get => _flashcards;
            set => SetProperty(ref _flashcards, value);
        }

        // Thẻ đang được chọn
        private Flashcard? _selectedFlashcard;
        public Flashcard? SelectedFlashcard
        {
            get => _selectedFlashcard;
            set => SetProperty(ref _selectedFlashcard, value);
        }

        // Tên deck mới (để tạo deck)
        private string _newDeckName = string.Empty;
        public string NewDeckName
        {
            get => _newDeckName;
            set => SetProperty(ref _newDeckName, value);
        }

        // Thuật ngữ mới (để tạo flashcard)
        private string _newTerm = string.Empty;
        public string NewTerm
        {
            get => _newTerm;
            set => SetProperty(ref _newTerm, value);
        }

        // Định nghĩa mới (để tạo flashcard)
        private string _newDefinition = string.Empty;
        public string NewDefinition
        {
            get => _newDefinition;
            set => SetProperty(ref _newDefinition, value);
        }

        // Thông báo trạng thái cho người dùng
        private string _statusMessage = string.Empty;
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        #endregion

        #region Commands (Các lệnh binding với View)

        // Command để load danh sách deck
        public ICommand LoadDecksCommand { get; }

        // Command để tạo deck mới
        public ICommand CreateDeckCommand { get; }

        // Command để xóa deck đang chọn
        public ICommand DeleteDeckCommand { get; }

        // Command để thêm thẻ mới vào deck
        public ICommand AddFlashcardCommand { get; }

        // Command để xóa thẻ đang chọn
        public ICommand DeleteFlashcardCommand { get; }

        // Command để toggle bookmark
        public ICommand ToggleBookmarkCommand { get; }

        #endregion

        /// <summary>
        /// Constructor - khởi tạo ViewModel
        /// </summary>
        public MainViewModel()
        {
            // Khởi tạo service từ BLL
            _deckService = new DeckService();

            // Khởi tạo các Command
            LoadDecksCommand = new RelayCommand(_ => LoadDecks());
            CreateDeckCommand = new RelayCommand(_ => CreateDeck(), _ => CanCreateDeck());
            DeleteDeckCommand = new RelayCommand(_ => DeleteDeck(), _ => SelectedDeck != null);
            AddFlashcardCommand = new RelayCommand(_ => AddFlashcard(), _ => CanAddFlashcard());
            DeleteFlashcardCommand = new RelayCommand(_ => DeleteFlashcard(), _ => SelectedFlashcard != null);
            ToggleBookmarkCommand = new RelayCommand<Flashcard>(ToggleBookmark);

            // Tự động load dữ liệu khi khởi tạo
            LoadDecks();
        }

        #region Command Methods (Các phương thức xử lý)

        /// <summary>
        /// Load tất cả deck từ BLL
        /// </summary>
        private void LoadDecks()
        {
            try
            {
                var decks = _deckService.GetAllDecks();
                Decks = new ObservableCollection<Deck>(decks);
                StatusMessage = $"Đã tải {decks.Count} bộ thẻ";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Lỗi: {ex.Message}";
            }
        }

        /// <summary>
        /// Load danh sách flashcard của deck đang chọn
        /// </summary>
        private void LoadFlashcards()
        {
            if (SelectedDeck != null)
            {
                Flashcards = new ObservableCollection<Flashcard>(SelectedDeck.Flashcards);
                StatusMessage = $"Bộ thẻ '{SelectedDeck.Name}' có {SelectedDeck.CardCount} thẻ";
            }
            else
            {
                Flashcards.Clear();
            }
        }

        /// <summary>
        /// Kiểm tra có thể tạo deck mới không
        /// </summary>
        private bool CanCreateDeck()
        {
            return !string.IsNullOrWhiteSpace(NewDeckName);
        }

        /// <summary>
        /// Tạo deck mới
        /// </summary>
        private void CreateDeck()
        {
            var newDeck = _deckService.CreateDeck(NewDeckName);
            if (newDeck != null)
            {
                Decks.Add(newDeck);
                NewDeckName = string.Empty; // Clear input
                StatusMessage = $"Đã tạo bộ thẻ '{newDeck.Name}'";
            }
            else
            {
                StatusMessage = "Không thể tạo bộ thẻ";
            }
        }

        /// <summary>
        /// Xóa deck đang chọn
        /// </summary>
        private void DeleteDeck()
        {
            if (SelectedDeck == null) return;

            // Hiện hộp thoại xác nhận
            var result = MessageBox.Show(
                $"Bạn có chắc muốn xóa bộ thẻ '{SelectedDeck.Name}'?",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (_deckService.DeleteDeck(SelectedDeck.Id))
                {
                    var deletedName = SelectedDeck.Name;
                    Decks.Remove(SelectedDeck);
                    SelectedDeck = null;
                    StatusMessage = $"Đã xóa bộ thẻ '{deletedName}'";
                }
            }
        }

        /// <summary>
        /// Kiểm tra có thể thêm flashcard không
        /// </summary>
        private bool CanAddFlashcard()
        {
            return SelectedDeck != null && !string.IsNullOrWhiteSpace(NewTerm);
        }

        /// <summary>
        /// Thêm flashcard mới vào deck đang chọn
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

                StatusMessage = $"Đã thêm thẻ '{newCard.Term}'";
            }
            else
            {
                StatusMessage = "Không thể thêm thẻ";
            }
        }

        /// <summary>
        /// Xóa flashcard đang chọn
        /// </summary>
        private void DeleteFlashcard()
        {
            if (SelectedDeck == null || SelectedFlashcard == null) return;

            if (_deckService.DeleteCard(SelectedDeck.Id, SelectedFlashcard.Id))
            {
                var deletedTerm = SelectedFlashcard.Term;
                Flashcards.Remove(SelectedFlashcard);
                SelectedFlashcard = null;
                StatusMessage = $"Đã xóa thẻ '{deletedTerm}'";
            }
        }

        /// <summary>
        /// Toggle bookmark cho một thẻ
        /// </summary>
        private void ToggleBookmark(Flashcard? card)
        {
            if (SelectedDeck == null || card == null) return;

            if (_deckService.ToggleBookmark(SelectedDeck.Id, card.Id))
            {
                card.IsBookmarked = !card.IsBookmarked;
                StatusMessage = card.IsBookmarked
                    ? $"Đã đánh dấu '{card.Term}'"
                    : $"Đã bỏ đánh dấu '{card.Term}'";
            }
        }

        #endregion
    }
}