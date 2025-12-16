using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
        public int PairId { get; set; } // ID cua the ghep doi
    }

    /// <summary>
    /// Cua so lam bai kiem tra dang ghep the
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

        public MatchingQuizWindow()
        {
            InitializeComponent();
            _deckService = new DeckService();
            LoadDecks();
        }

        /// <summary>
        /// Load danh sach deck
        /// </summary>
        private void LoadDecks()
        {
            var decks = _deckService.GetAllDecks();
            lstDecks.ItemsSource = decks;
        }

        /// <summary>
        /// Xu ly khi chon deck
        /// </summary>
        private void LstDecks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstDecks.SelectedItem is Deck selectedDeck)
            {
                _selectedDeck = selectedDeck;
                txtDeckName.Text = $"Bo the: {selectedDeck.Name} ({selectedDeck.CardCount} the)";
                btnStartQuiz.IsEnabled = selectedDeck.CardCount >= 4; // Can it nhat 4 the
            }
        }

        /// <summary>
        /// Bat dau quiz
        /// </summary>
        private void BtnStartQuiz_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedDeck == null || _selectedDeck.Flashcards == null || _selectedDeck.Flashcards.Count < 4)
            {
                MessageBox.Show("Bo the phai co it nhat 4 the!", "Thong bao",
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

            // Reset trang thai
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
                var timer = new System.Windows.Threading.DispatcherTimer
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
            txtProgress.Text = $"Tien do: {_matchedPairs} / {total}";
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

            txtResult.Text = $"Hoan thanh! Diem: {score:F1}";
            txtResultDetail.Text = $"Thoi gian: {elapsed.TotalSeconds:F1}s | Luot thu: {_attempts}";
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
            double attemptPenalty = wrongAttempts * 0.5;

            double finalScore = baseScore - timePenalty - attemptPenalty;
            return Math.Max(0, finalScore);
        }

        /// <summary>
        /// Lam lai bai quiz
        /// </summary>
        private void BtnRestart_Click(object sender, RoutedEventArgs e)
        {
            StartMatchingQuiz();
        }

        /// <summary>
        /// Quay lai
        /// </summary>
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            var quizWindow = new QuizWindow();
            quizWindow.Show();
            this.Close();
        }
    }
}
