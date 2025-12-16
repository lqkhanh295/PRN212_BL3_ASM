using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using ASM.Bussiness.Services;
using ASM.Entities.Models;

namespace ASM_PRN212_BL3.Views
{
    /// <summary>
    /// Cửa sổ học flashcard - chế độ Quiz cho sinh viên
    /// </summary>
    public partial class QuizWindow : Window
    {
        private readonly DeckService _deckService;
        private readonly User? _currentUser;

        // Danh sách thẻ đang học
        private List<Flashcard> _currentCards = new();
        private int _currentIndex = 0;
        private bool _isShowingDefinition = false;

        // Theo dõi kết quả
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
        /// Khởi tạo giao diện
        /// </summary>
        private void InitializeUI()
        {
            // Hiển thị tên người dùng
            if (_currentUser != null)
            {
                txtWelcome.Text = $"Xin chào, {_currentUser.DisplayName}!";
            }
            else
            {
                txtWelcome.Text = "Xin chào, Khách!";
            }

            // Ẩn các nút khi chưa chọn deck
            UpdateNavigationButtons();
        }

        /// <summary>
        /// Load danh sách deck
        /// </summary>
        private void LoadDecks()
        {
            var decks = _deckService.GetAllDecks();
            lstDecks.ItemsSource = decks;
        }

        /// <summary>
        /// Xử lý khi chọn deck
        /// </summary>
        private void LstDecks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstDecks.SelectedItem is Deck selectedDeck)
            {
                StartLearning(selectedDeck);
            }
        }

        /// <summary>
        /// Bắt đầu học một bộ thẻ
        /// </summary>
        private void StartLearning(Deck deck)
        {
            if (deck.Flashcards == null || deck.Flashcards.Count == 0)
            {
                MessageBox.Show("Bộ thẻ này chưa có thẻ nào!", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Reset trạng thái
            _currentCards = new List<Flashcard>(deck.Flashcards);
            _currentIndex = 0;
            _isShowingDefinition = false;
            _correctCount = 0;
            _wrongCount = 0;
            _answeredCards.Clear();

            // Ẩn panel kết quả
            pnlResult.Visibility = Visibility.Collapsed;
            pnlAssessment.Visibility = Visibility.Collapsed;

            // Hiển thị thẻ đầu tiên
            DisplayCurrentCard();
            UpdateNavigationButtons();
        }

        /// <summary>
        /// Hiển thị thẻ hiện tại
        /// </summary>
        private void DisplayCurrentCard()
        {
            if (_currentCards.Count == 0 || _currentIndex < 0 || _currentIndex >= _currentCards.Count)
            {
                txtCardContent.Text = "Không có thẻ nào";
                txtCardLabel.Text = "";
                return;
            }

            var card = _currentCards[_currentIndex];

            // Hiển thị mặt trước hoặc mặt sau
            if (_isShowingDefinition)
            {
                txtCardLabel.Text = "ĐỊNH NGHĨA";
                txtCardContent.Text = card.Definition;
                cardBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8F5E9"));

                // Hiển thị nút đánh giá nếu chưa trả lời
                if (!_answeredCards.Contains(card.Id))
                {
                    pnlAssessment.Visibility = Visibility.Visible;
                }
            }
            else
            {
                txtCardLabel.Text = "THUẬT NGỮ";
                txtCardContent.Text = card.Term;
                cardBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FAFBFC"));
                pnlAssessment.Visibility = Visibility.Collapsed;
            }

            // Hiển thị bookmark
            txtBookmark.Visibility = card.IsBookmarked ? Visibility.Visible : Visibility.Collapsed;

            // Cập nhật progress
            UpdateProgress();
        }

        /// <summary>
        /// Cập nhật thanh tiến trình
        /// </summary>
        private void UpdateProgress()
        {
            txtProgress.Text = $"Thẻ: {_currentIndex + 1} / {_currentCards.Count}";
            progressBar.Maximum = _currentCards.Count;
            progressBar.Value = _currentIndex + 1;
        }

        /// <summary>
        /// Cập nhật trạng thái các nút điều hướng
        /// </summary>
        private void UpdateNavigationButtons()
        {
            bool hasCards = _currentCards.Count > 0;
            btnPrevious.IsEnabled = hasCards && _currentIndex > 0;
            btnNext.IsEnabled = hasCards && _currentIndex < _currentCards.Count - 1;
            btnShuffle.IsEnabled = hasCards;
        }

        /// <summary>
<<<<<<< HEAD
        /// Lật thẻ (click vào thẻ)
=======
        /// Lat the (click vao the) voi animation
>>>>>>> 5b835f85684f5e91133de1435d46ffa8ac8bc8b7
        /// </summary>
        private void CardBorder_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_currentCards.Count == 0) return;

            // Tao animation flip - scale xuong 0
            var flipOut = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromMilliseconds(150),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
            };

            // Khi animation scale xuong xong, doi noi dung va scale len lai
            flipOut.Completed += (s, args) =>
            {
                _isShowingDefinition = !_isShowingDefinition;
                DisplayCurrentCard();

                var flipIn = new DoubleAnimation
                {
                    From = 0.0,
                    To = 1.0,
                    Duration = TimeSpan.FromMilliseconds(150),
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                };

                CardScale.BeginAnimation(ScaleTransform.ScaleXProperty, flipIn);
            };

            CardScale.BeginAnimation(ScaleTransform.ScaleXProperty, flipOut);
        }

        /// <summary>
        /// Thẻ trước
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
        /// Thẻ sau
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
                // Đã hết thẻ -> hiển thị kết quả
                ShowResults();
            }
        }

        /// <summary>
        /// Trộn ngẫu nhiên các thẻ
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

            // Reset về thẻ đầu
            _currentIndex = 0;
            _isShowingDefinition = false;
            DisplayCurrentCard();
            UpdateNavigationButtons();

            MessageBox.Show("Đã trộn ngẫu nhiên các thẻ!", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Đánh dấu là chưa thuộc
        /// </summary>
        private void BtnWrong_Click(object sender, RoutedEventArgs e)
        {
            MarkAnswer(false);
        }

        /// <summary>
        /// Đánh dấu là đã thuộc
        /// </summary>
        private void BtnCorrect_Click(object sender, RoutedEventArgs e)
        {
            MarkAnswer(true);
        }

        /// <summary>
        /// Ghi nhận câu trả lời
        /// </summary>
        private void MarkAnswer(bool isCorrect)
        {
            if (_currentCards.Count == 0) return;

            var card = _currentCards[_currentIndex];

            // Chỉ ghi nhận mỗi thẻ một lần
            if (!_answeredCards.Contains(card.Id))
            {
                _answeredCards.Add(card.Id);
                if (isCorrect)
                    _correctCount++;
                else
                    _wrongCount++;
            }

            // Ẩn nút đánh giá
            pnlAssessment.Visibility = Visibility.Collapsed;

            // Tự động chuyển thẻ tiếp theo
            if (_currentIndex < _currentCards.Count - 1)
            {
                _currentIndex++;
                _isShowingDefinition = false;
                DisplayCurrentCard();
                UpdateNavigationButtons();
            }
            else
            {
                // Hết thẻ -> hiển thị kết quả
                ShowResults();
            }
        }

        /// <summary>
        /// Hiển thị kết quả học tập
        /// </summary>
        private void ShowResults()
        {
            int total = _answeredCards.Count;
            if (total == 0)
            {
                MessageBox.Show("Bạn chưa học thẻ nào cả!", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            double percent = (double)_correctCount / total * 100;

            txtResult.Text = $"Kết quả: {_correctCount}/{total} ({percent:F1}%)";
            txtResultDetail.Text = $"Đã thuộc: {_correctCount} | Chưa thuộc: {_wrongCount}";

            // Đổi màu theo kết quả
            if (percent >= 80)
            {
                pnlResult.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#27AE60"));
                txtResult.Foreground = new SolidColorBrush(Colors.White);
            }
            else if (percent >= 50)
            {
                pnlResult.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F39C12"));
                txtResult.Foreground = new SolidColorBrush(Colors.White);
            }
            else
            {
                pnlResult.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E74C3C"));
                txtResult.Foreground = new SolidColorBrush(Colors.White);
            }

            pnlResult.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Học lại từ đầu
        /// </summary>
        private void BtnRestart_Click(object sender, RoutedEventArgs e)
        {
            if (lstDecks.SelectedItem is Deck selectedDeck)
            {
                StartLearning(selectedDeck);
            }
        }

        /// <summary>
        /// Đăng xuất
        /// </summary>
        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        /// <summary>
        /// Chuyen sang che do ghep the
        /// </summary>
        private void BtnMatchingMode_Click(object sender, RoutedEventArgs e)
        {
            var matchingWindow = new MatchingQuizWindow();
            matchingWindow.Show();
            this.Close();
        }
    }
}