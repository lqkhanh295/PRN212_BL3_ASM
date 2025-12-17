using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using ASM.Bussiness.Services;
using ASM.Entities.Models;

namespace ASM_PRN212_BL3.Views
{
    /// <summary>
    /// Matching card item for display
    /// </summary>
    public class MatchingCard
    {
        public int Id { get; set; }
        public string Label { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Background { get; set; } = "#FAFBFC";
        public bool IsMatched { get; set; }
        public int PairId { get; set; } // ID c?a th? ghép ?ôi
    }

    /// <summary>
    /// C?a s? làm bài ki?m tra d?ng ghép th?
    /// </summary>
    public partial class MatchingQuizWindow : Window
    {
        private readonly DeckService _deckService;
        private Deck? _selectedDeck;
        private List<MatchingCard> _matchingCards = new();
        private MatchingCard? _firstSelectedCard;
        private int _matchedPairs = 0;
        private DateTime _startTime;
        private int _attempts = 0;

        // Timer ??m ng??c
        private DispatcherTimer? _countdownTimer;
        private int _remainingSeconds = 60; //60s m?c ??nh

        public MatchingQuizWindow()
        {
            try
            {
                InitializeComponent();
                // Ensure window state is Normal on open
                this.WindowState = WindowState.Normal;
                this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                _deckService = new DeckService();
                LoadDecks();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"L?i khi kh?i t?o MatchingQuizWindow:\n{ex.Message}\n\n{ex.StackTrace}", "L?i", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Không override các text ?ã ??nh ngh?a trong XAML ?? tránh v?n ?? mã hoá
            // N?u c?n c?p nh?t ??ng, hãy gán các chu?i Unicode ?úng tr?c ti?p
            txtTimer.Text = "60s";
        }

        /// <summary>
        /// Load danh sách deck
        /// </summary>
        private void LoadDecks()
        {
            var decks = _deckService.GetAllDecks();
            lstDecks.ItemsSource = decks;
        }

        private void StartCountdown()
        {
            _remainingSeconds = 60;
            txtTimer.Text = $"{_remainingSeconds}s";
            _countdownTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _countdownTimer.Tick += (s, e) =>
            {
                _remainingSeconds--;
                txtTimer.Text = $"{_remainingSeconds}s";
                if (_remainingSeconds <= 0)
                {
                    _countdownTimer.Stop();
                    // Khi h?t gi?, ch?m ?i?m ngay
                    ShowResult();
                }
            };
            _countdownTimer.Start();
        }

        private void StopCountdown()
        {
            if (_countdownTimer != null)
            {
                _countdownTimer.Stop();
                _countdownTimer = null;
            }
        }

        /// <summary>
        /// X? lý khi ch?n deck
        /// </summary>
        private void LstDecks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstDecks.SelectedItem is Deck selectedDeck)
            {
                _selectedDeck = selectedDeck;
                // Update Runs defined in XAML to preserve Vietnamese diacritics
                try
                {
                    runDeckInfo.Text = selectedDeck.Name ?? string.Empty;
                    runDeckCount.Text = selectedDeck.CardCount.ToString();
                }
                catch
                {
                    // If Runs not found for some reason, set plain Text as fallback
                    txtDeckName.Text = $"B? th?: {selectedDeck.Name} ({selectedDeck.CardCount} th?)";
                }
                btnStartQuiz.IsEnabled = selectedDeck.CardCount >= 4; // C?n ít nh?t 4 th?
            }
        }

        /// <summary>
        /// B?t ??u quiz
        /// </summary>
        private void BtnStartQuiz_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedDeck == null || _selectedDeck.Flashcards == null || _selectedDeck.Flashcards.Count < 4)
            {
                MessageBox.Show("B? th? ph?i có ít nh?t 4 th?!", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            StartMatchingQuiz();
        }

        /// <summary>
        /// Khoi tao va hien thi cac the ghep
        /// </summary>
        private void StartMatchingQuiz()
        {
            if (_selectedDeck?.Flashcards == null) return;

            // Reset tr?ng thái
            _matchingCards.Clear();
            _firstSelectedCard = null;
            _matchedPairs = 0;
            _attempts = 0;
            _startTime = DateTime.Now;
            pnlResult.Visibility = Visibility.Collapsed;
            txtInitialMessage.Visibility = Visibility.Collapsed;

            // Lay ngau nhien toi da 6 the (6 cap = 12 o)
            var random = new Random();
            var selectedFlashcards = _selectedDeck.Flashcards
                .OrderBy(x => random.Next())
                .Take(6)
                .ToList();

            int cardId = 1;

            // Tao cac the ghep: Term va Definition
            foreach (var flashcard in selectedFlashcards)
            {
                // The term (cau hoi)
                _matchingCards.Add(new MatchingCard
                {
                    Id = cardId,
                    Label = $"Q.{cardId}",
                    Content = flashcard.Term,
                    Background = "#E3F2FD",
                    PairId = cardId + 100 // Ghep voi the definition
                });

                // The definition (dap an)
                _matchingCards.Add(new MatchingCard
                {
                    Id = cardId + 100,
                    Label = "", // Khong hien thi label cho dap an
                    Content = flashcard.Definition,
                    Background = "#FFF3E0",
                    PairId = cardId // Ghep voi the term
                });

                cardId++;
            }

            // Tron ngau nhien
            _matchingCards = _matchingCards.OrderBy(x => random.Next()).ToList();

            // Hien thi len UI
            itemsMatching.ItemsSource = _matchingCards;

            // Cap nhat progress
            UpdateProgress();

            // B?t ??u ??m ng??c 60s
            StartCountdown();
        }

        /// <summary>
        /// Xu ly khi click vao mot the
        /// </summary>
        private void Card_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is not Border border) return;
            if (border.DataContext is not MatchingCard clickedCard) return;
            if (clickedCard.IsMatched) return; // Da ghep roi thi khong lam gi

            // Neu chua co the nao duoc chon
            if (_firstSelectedCard == null)
            {
                _firstSelectedCard = clickedCard;
                border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF59D")); // Highlight
                return;
            }

            // Neu click lai chinh no
            if (_firstSelectedCard.Id == clickedCard.Id)
            {
                _firstSelectedCard = null;
                ResetCardColor(border, clickedCard);
                return;
            }

            // Kiem tra xem co ghep dung khong
            _attempts++;

            if (_firstSelectedCard.PairId == clickedCard.Id)
            {
                // Ghep dung!
                _firstSelectedCard.IsMatched = true;
                clickedCard.IsMatched = true;
                _matchedPairs++;

                // Doi mau thanh xanh
                SetCardMatched(border);
                SetCardMatchedByCard(_firstSelectedCard);

                _firstSelectedCard = null;

                // Kiem tra xem da ghep het chua
                if (_matchedPairs == _matchingCards.Count / 2)
                {
                    StopCountdown();
                    ShowResult();
                }
                else
                {
                    UpdateProgress();
                }
            }
            else
            {
                // Ghep sai - hien thi do roi reset
                border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCDD2"));
                SetCardWrong(_firstSelectedCard);

                // Reset sau 500ms
                var timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(500)
                };
                timer.Tick += (s, args) =>
                {
                    ResetCardColor(border, clickedCard);
                    ResetCardColorByCard(_firstSelectedCard);
                    _firstSelectedCard = null;
                    timer.Stop();
                };
                timer.Start();
            }
        }

        /// <summary>
        /// Dat mau cho the ghep dung
        /// </summary>
        private void SetCardMatched(Border border)
        {
            border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C8E6C9"));
        }

        /// <summary>
        /// Dat mau cho the ghep dung theo card
        /// </summary>
        private void SetCardMatchedByCard(MatchingCard card)
        {
            var container = FindCardBorder(card);
            if (container != null)
            {
                container.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C8E6C9"));
            }
        }

        /// <summary>
        /// Dat mau cho the ghep sai
        /// </summary>
        private void SetCardWrong(MatchingCard? card)
        {
            if (card == null) return;
            var container = FindCardBorder(card);
            if (container != null)
            {
                container.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCDD2"));
            }
        }

        /// <summary>
        /// Reset mau the ve ban dau
        /// </summary>
        private void ResetCardColor(Border border, MatchingCard card)
        {
            var originalColor = card.Label.StartsWith("Q.") ? "#E3F2FD" : "#FFF3E0";
            border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(originalColor));
        }

        /// <summary>
        /// Reset mau the ve ban dau theo card
        /// </summary>
        private void ResetCardColorByCard(MatchingCard? card)
        {
            if (card == null) return;
            var container = FindCardBorder(card);
            if (container != null)
            {
                var originalColor = card.Label.StartsWith("Q.") ? "#E3F2FD" : "#FFF3E0";
                container.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(originalColor));
            }
        }

        /// <summary>
        /// Tim Border container cua mot card
        /// </summary>
        private Border? FindCardBorder(MatchingCard card)
        {
            if (itemsMatching.ItemsSource == null) return null;

            foreach (var item in itemsMatching.Items)
            {
                var container = itemsMatching.ItemContainerGenerator.ContainerFromItem(item);
                if (container is ContentPresenter presenter)
                {
                    var border = FindVisualChild<Border>(presenter);
                    if (border?.DataContext == card)
                    {
                        return border;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Tim child element theo kieu
        /// </summary>
        private T? FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T result)
                    return result;

                var childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null)
                    return childOfChild;
            }
            return null;
        }

        /// <summary>
        /// Cap nhat progress bar
        /// </summary>
        private void UpdateProgress()
        {
            int total = _matchingCards.Count / 2;
            txtProgress.Text = $"Ti?n ??: {_matchedPairs} / {total}";
            progressBar.Maximum = total;
            progressBar.Value = _matchedPairs;
        }

        /// <summary>
        /// Hien thi ket qua
        /// </summary>
        private void ShowResult()
        {
            var elapsed = DateTime.Now - _startTime;
            var score = CalculateScore(elapsed, _attempts);

            txtResult.Text = $"Hoàn thành! ?i?m: {score:F1}";
            txtResultDetail.Text = $"Th?i gian: {elapsed.TotalSeconds:F1}s | L??t th?: {_attempts}";
            txtScore.Text = score.ToString("F1");

            pnlResult.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Tinh diem dua tren thoi gian va so lan thu
        /// </summary>
        private double CalculateScore(TimeSpan elapsed, int attempts)
        {
            int totalPairs = _matchingCards.Count / 2;
            double baseScore = 10.0;

            // Tru diem theo thoi gian (toi da -3 diem)
            double timePenalty = Math.Min(elapsed.TotalSeconds / 10.0, 3.0);

            // Tru diem theo so lan sai (moi lan sai = attempts - totalPairs)
            int wrongAttempts = attempts - totalPairs;
            double attemptPenalty = Math.Max(0, wrongAttempts) * 0.5;

            double finalScore = baseScore - timePenalty - attemptPenalty;
            return Math.Max(0, finalScore);
        }

        /// <summary>
        /// Lam lai bai quiz
        /// </summary>
        private void BtnRestart_Click(object sender, RoutedEventArgs e)
        {
            StopCountdown();
            StartMatchingQuiz();
        }

        /// <summary>
        /// Quay lai
        /// </summary>
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            StopCountdown();
            var quizWindow = new QuizWindow();
            quizWindow.Show();
            this.Close();
        }
    }
}
