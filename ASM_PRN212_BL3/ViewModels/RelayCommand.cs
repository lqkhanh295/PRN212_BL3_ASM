using System.Windows.Input;

namespace ASM_PRN212_BL3.ViewModels
{
    /// <summary>
    /// Implementation c?a ICommand ?? s? d?ng trong MVVM
    /// Cho phép binding các hành ??ng t? View ??n ViewModel
    /// </summary>
    public class RelayCommand : ICommand
    {
        // Delegate ch?a hành ??ng c?n th?c thi
        private readonly Action<object?> _execute;

        // Delegate ki?m tra xem command có th? th?c thi không
        private readonly Predicate<object?>? _canExecute;

        /// <summary>
        /// Constructor v?i hành ??ng th?c thi
        /// </summary>
        /// <param name="execute">Hành ??ng c?n th?c thi khi command ???c g?i</param>
        public RelayCommand(Action<object?> execute) : this(execute, null)
        {
        }

        /// <summary>
        /// Constructor v?i hành ??ng th?c thi và ?i?u ki?n
        /// </summary>
        /// <param name="execute">Hành ??ng c?n th?c thi</param>
        /// <param name="canExecute">?i?u ki?n ?? command có th? th?c thi</param>
        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Event ???c g?i khi ?i?u ki?n CanExecute có th? ?ã thay ??i
        /// WPF t? ??ng l?ng nghe và c?p nh?t tr?ng thái enabled/disabled c?a control
        /// </summary>
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Ki?m tra xem command có th? th?c thi không
        /// </summary>
        /// <param name="parameter">Tham s? truy?n vào (có th? null)</param>
        /// <returns>True n?u có th? th?c thi</returns>
        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <summary>
        /// Th?c thi command
        /// </summary>
        /// <param name="parameter">Tham s? truy?n vào</param>
        public void Execute(object? parameter)
        {
            _execute(parameter);
        }

        /// <summary>
        /// G?i ?? thông báo CanExecute có th? ?ã thay ??i
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }

    /// <summary>
    /// RelayCommand generic cho các tr??ng h?p c?n type-safe parameter
    /// </summary>
    /// <typeparam name="T">Ki?u c?a parameter</typeparam>
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
