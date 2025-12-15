using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ASM.Bussiness.Services;
using ASM.Entities.Models;

namespace ASM_PRN212_BL3.Views
{
    /// <summary>
    /// Cua so hoc flashcard - che do Quiz cho sinh vien
    /// </summary>
    public partial class QuizWindow : Window
    {
        private readonly DeckService _deckService;
        private readonly User? _currentUser;

        // Danh sach the dang hoc
        private List<Flashcard> _currentCards = new();
        private int _currentIndex = 0;
        private bool _isShowingDefinition = false;

        // Theo doi ket qua
        private int _correctCount = 0;
        private int _wrongCount = 0;
        private HashSet<int> _answeredCards = new();

        public QuizWindow(User? user = null)
        {
            InitializeComponent();
            _deckService = new DeckService();
            _currentUser = user;

            InitializeUI();
            LoadDecks();
        }

        /// <summary>
        /// Khoi tao giao dien
        /// </summary>
        private void InitializeUI()
        {
            // Hien thi ten nguoi dung
            if (_currentUser != null)
            {
                txtWelcome.Text = $"Xin chao, {_currentUser.DisplayName}!";
            }
            else
            {
                txtWelcome.Text = "Xin chao, Khach!";
            }

            // An cac nut khi chua chon deck
            UpdateNavigationButtons();
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
                StartLearning(selectedDeck);
            }
        }

        /// <summary>
        /// Bat dau hoc mot bo the
        /// </summary>
        private void StartLearning(Deck deck)
        {
            if (deck.Flashcards == null || deck.Flashcards.Count == 0)
            {
                MessageBox.Show("Bo the nay chua co the nao!", "Thong bao", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Reset trang thai
            _currentCards = new List<Flashcard>(deck.Flashcards);
            _currentIndex = 0;
            _isShowingDefinition = false;
            _correctCount = 0;
            _wrongCount = 0;
            _answeredCards.Clear();

            // An panel ket qua
            pnlResult.Visibility = Visibility.Collapsed;
            pnlAssessment.Visibility = Visibility.Collapsed;

            // Hien thi the dau tien
            DisplayCurrentCard();
            UpdateNavigationButtons();
        }

        /// <summary>
        /// Hien thi the hien tai
        /// </summary>
        private void DisplayCurrentCard()
        {
            if (_currentCards.Count == 0 || _currentIndex < 0 || _currentIndex >= _currentCards.Count)
            {
                txtCardContent.Text = "Khong co the nao";
                txtCardLabel.Text = "";
                return;
            }

            var card = _currentCards[_currentIndex];

            // Hien thi mat truoc hoac mat sau
            if (_isShowingDefinition)
            {
                txtCardLabel.Text = "DINH NGHIA";
                txtCardContent.Text = card.Definition;
                cardBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8F5E9"));
                
                // Hien thi nut danh gia neu chua tra loi
                if (!_answeredCards.Contains(card.Id))
                {
                    pnlAssessment.Visibility = Visibility.Visible;
                }
            }
            else
            {
                txtCardLabel.Text = "THUAT NGU";
                txtCardContent.Text = card.Term;
                cardBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F8F9FA"));
                pnlAssessment.Visibility = Visibility.Collapsed;
            }

            // Hien thi bookmark
            txtBookmark.Visibility = card.IsBookmarked ? Visibility.Visible : Visibility.Collapsed;

            // Cap nhat progress
            UpdateProgress();
        }

        /// <summary>
        /// Cap nhat thanh tien trinh
        /// </summary>
        private void UpdateProgress()
        {
            txtProgress.Text = $"The: {_currentIndex + 1} / {_currentCards.Count}";
            progressBar.Maximum = _currentCards.Count;
            progressBar.Value = _currentIndex + 1;
        }

        /// <summary>
        /// Cap nhat trang thai cac nut dieu huong
        /// </summary>
        private void UpdateNavigationButtons()
        {
            bool hasCards = _currentCards.Count > 0;
            btnPrevious.IsEnabled = hasCards && _currentIndex > 0;
            btnNext.IsEnabled = hasCards && _currentIndex < _currentCards.Count - 1;
            btnShuffle.IsEnabled = hasCards;
        }

        /// <summary>
        /// Lat the (click vao the)
        /// </summary>
        private void CardBorder_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_currentCards.Count == 0) return;

            _isShowingDefinition = !_isShowingDefinition;
            DisplayCurrentCard();
        }

        /// <summary>
        /// The truoc
        /// </summary>
        private void BtnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (_currentIndex > 0)
            {
                _currentIndex--;
                _isShowingDefinition = false;
                DisplayCurrentCard();
                UpdateNavigationButtons();
            }
        }

        /// <summary>
        /// The sau
        /// </summary>
        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            if (_currentIndex < _currentCards.Count - 1)
            {
                _currentIndex++;
                _isShowingDefinition = false;
                DisplayCurrentCard();
                UpdateNavigationButtons();
            }
            else
            {
                // Da het the -> hien thi ket qua
                ShowResults();
            }
        }

        /// <summary>
        /// Tron ngau nhien cac the
        /// </summary>
        private void BtnShuffle_Click(object sender, RoutedEventArgs e)
        {
            if (_currentCards.Count == 0) return;

            // Fisher-Yates shuffle
            var random = new Random();
            for (int i = _currentCards.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (_currentCards[i], _currentCards[j]) = (_currentCards[j], _currentCards[i]);
            }

            // Reset ve the dau
            _currentIndex = 0;
            _isShowingDefinition = false;
            DisplayCurrentCard();
            UpdateNavigationButtons();

            MessageBox.Show("Da tron ngau nhien cac the!", "Thong bao", 
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Danh dau la chua thuoc
        /// </summary>
        private void BtnWrong_Click(object sender, RoutedEventArgs e)
        {
            MarkAnswer(false);
        }

        /// <summary>
        /// Danh dau la da thuoc
        /// </summary>
        private void BtnCorrect_Click(object sender, RoutedEventArgs e)
        {
            MarkAnswer(true);
        }

        /// <summary>
        /// Ghi nhan cau tra loi
        /// </summary>
        private void MarkAnswer(bool isCorrect)
        {
            if (_currentCards.Count == 0) return;

            var card = _currentCards[_currentIndex];
            
            // Chi ghi nhan moi the mot lan
            if (!_answeredCards.Contains(card.Id))
            {
                _answeredCards.Add(card.Id);
                if (isCorrect)
                    _correctCount++;
                else
                    _wrongCount++;
            }

            // An nut danh gia
            pnlAssessment.Visibility = Visibility.Collapsed;

            // Tu dong chuyen the tiep theo
            if (_currentIndex < _currentCards.Count - 1)
            {
                _currentIndex++;
                _isShowingDefinition = false;
                DisplayCurrentCard();
                UpdateNavigationButtons();
            }
            else
            {
                // Het the -> hien thi ket qua
                ShowResults();
            }
        }

        /// <summary>
        /// Hien thi ket qua hoc tap
        /// </summary>
        private void ShowResults()
        {
            int total = _answeredCards.Count;
            if (total == 0)
            {
                MessageBox.Show("Ban chua hoc the nao ca!", "Thong bao", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            double percent = (double)_correctCount / total * 100;

            txtResult.Text = $"Ket qua: {_correctCount}/{total} ({percent:F1}%)";
            txtResultDetail.Text = $"Da thuoc: {_correctCount} | Chua thuoc: {_wrongCount}";

            // Doi mau theo ket qua
            if (percent >= 80)
            {
                pnlResult.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#28A745"));
            }
            else if (percent >= 50)
            {
                pnlResult.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC107"));
                txtResult.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                pnlResult.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC3545"));
            }

            pnlResult.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Hoc lai tu dau
        /// </summary>
        private void BtnRestart_Click(object sender, RoutedEventArgs e)
        {
            if (lstDecks.SelectedItem is Deck selectedDeck)
            {
                StartLearning(selectedDeck);
            }
        }

        /// <summary>
        /// Dang xuat
        /// </summary>
        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
