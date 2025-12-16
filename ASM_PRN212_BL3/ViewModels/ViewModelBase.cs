using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ASM_PRN212_BL3.ViewModels
{
    /// <summary>
    /// Lớp cơ sở cho tất cả ViewModel
    /// Cung cấp cơ chế thông báo thay đổi thuộc tính (INotifyPropertyChanged)
    /// Đây là nền tảng của MVVM pattern
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Event được kích hoạt khi một thuộc tính thay đổi giá trị
        /// WPF sẽ lắng nghe event này để tự động cập nhật giao diện
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Phương thức gọi để thông báo thuộc tính đã thay đổi
        /// </summary>
        /// <param name="propertyName">Tên thuộc tính (tự động lấy từ caller)</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Phương thức helper để set giá trị và tự động raise event
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu của thuộc tính</typeparam>
        /// <param name="field">Biến backing field</param>
        /// <param name="value">Giá trị mới</param>
        /// <param name="propertyName">Tên thuộc tính</param>
        /// <returns>True nếu giá trị thay đổi</returns>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            // Nếu giá trị không thay đổi, không làm gì
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            // Cập nhật giá trị và thông báo
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}