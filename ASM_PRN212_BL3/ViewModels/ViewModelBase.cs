using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ASM_PRN212_BL3.ViewModels
{
    /// <summary>
    /// L?p c? s? cho t?t c? ViewModel
    /// Cung c?p c? ch? thông báo thay ??i thu?c tính (INotifyPropertyChanged)
    /// ?ây là n?n t?ng c?a MVVM pattern
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Event ???c kích ho?t khi m?t thu?c tính thay ??i giá tr?
        /// WPF s? l?ng nghe event này ?? t? ??ng c?p nh?t giao di?n
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Ph??ng th?c g?i ?? thông báo thu?c tính ?ã thay ??i
        /// </summary>
        /// <param name="propertyName">Tên thu?c tính (t? ??ng l?y t? caller)</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Ph??ng th?c helper ?? set giá tr? và t? ??ng raise event
        /// </summary>
        /// <typeparam name="T">Ki?u d? li?u c?a thu?c tính</typeparam>
        /// <param name="field">Bi?n backing field</param>
        /// <param name="value">Giá tr? m?i</param>
        /// <param name="propertyName">Tên thu?c tính</param>
        /// <returns>True n?u giá tr? thay ??i</returns>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            // N?u giá tr? không thay ??i, không làm gì
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            // C?p nh?t giá tr? và thông báo
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
