using System.Windows.Input;

namespace ASM_PRN212_BL3.ViewModels
{
    /// <summary>
    /// Implementation của ICommand để sử dụng trong MVVM
    /// Cho phép binding các hành động từ View đến ViewModel
    /// </summary>
    public class RelayCommand : ICommand
    {
        // Delegate chứa hành động cần thực thi
        private readonly Action<object?> _execute;

        // Delegate kiểm tra xem command có thể thực thi không
        private readonly Predicate<object?>? _canExecute;

        /// <summary>
        /// Constructor với hành động thực thi
        /// </summary>
        /// <param name="execute">Hành động cần thực thi khi command được gọi</param>
        public RelayCommand(Action<object?> execute) : this(execute, null)
        {
        }

        /// <summary>
        /// Constructor với hành động thực thi và điều kiện
        /// </summary>
        /// <param name="execute">Hành động cần thực thi</param>
        /// <param name="canExecute">Điều kiện để command có thể thực thi</param>
        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Event được gọi khi điều kiện CanExecute có thể đã thay đổi
        /// WPF tự động lắng nghe và cập nhật trạng thái enabled/disabled của control
        /// </summary>
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Kiểm tra xem command có thể thực thi không
        /// </summary>
        /// <param name="parameter">Tham số truyền vào (có thể null)</param>
        /// <returns>True nếu có thể thực thi</returns>
        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <summary>
        /// Thực thi command
        /// </summary>
        /// <param name="parameter">Tham số truyền vào</param>
        public void Execute(object? parameter)
        {
            _execute(parameter);
        }

        /// <summary>
        /// Gọi để thông báo CanExecute có thể đã thay đổi
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }

    /// <summary>
    /// RelayCommand generic cho các trường hợp cần type-safe parameter
    /// </summary>
    /// <typeparam name="T">Kiểu của parameter</typeparam>
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T?> _execute;
        private readonly Predicate<T?>? _canExecute;

        public RelayCommand(Action<T?> execute) : this(execute, null)
        {
        }

        public RelayCommand(Action<T?> execute, Predicate<T?>? canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute((T?)parameter);
        }

        public void Execute(object? parameter)
        {
            _execute((T?)parameter);
        }
    }
}