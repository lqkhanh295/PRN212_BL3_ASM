Flashcard Quiz Application
Dự án này là một ứng dụng quản lý Flashcard được phát triển bằng C# và WPF (Windows Presentation Foundation) theo mô hình MVVM (Model-View-ViewModel). Ứng dụng hỗ trợ hai vai trò người dùng chính: Admin (quản lý bộ thẻ) và Student (học và làm bài kiểm tra).

🚀 Các tính năng chính
Dành cho Admin
Quản lý bộ thẻ (Deck): Tạo mới, xem danh sách và xóa các bộ thẻ học tập.

Quản lý Flashcard: Thêm, sửa, xóa các cặp thuật ngữ và định nghĩa trong mỗi bộ thẻ.

Import Excel: Nhập dữ liệu flashcard nhanh chóng từ tệp Excel (.xlsx).

Đánh dấu (Bookmark): Đánh dấu các thẻ quan trọng để ôn tập.

Dành cho Student
Chế độ Học (Quiz): Lật thẻ để xem nội dung, tự đánh giá "Đã thuộc" hoặc "Chưa thuộc" và xem kết quả học tập sau khi hoàn thành.

Chế độ Ghép thẻ (Matching): Trò chơi ghép thẻ thú vị để kiểm tra trí nhớ và tốc độ phản xạ.

Trộn thẻ (Shuffle): Thay đổi thứ tự các thẻ để nâng cao hiệu quả ôn tập.

🏗 Kiến trúc dự án
Dự án được chia thành các tầng rõ ràng (N-tier architecture) để dễ dàng bảo trì và mở rộng:

ASM.Entities: Chứa các lớp mô hình (Models) như Deck, Flashcard, User, QuizResult.

ASM.Data (DAL): Tầng truy cập dữ liệu, thực hiện đọc/ghi dữ liệu từ tệp JSON (data.json, users.json).

ASM.Bussiness (BLL): Tầng xử lý nghiệp vụ, trung gian giữa UI và Data.

ASM_PRN212_BL3 (WPF UI): Tầng giao diện người dùng, sử dụng MVVM pattern với các ViewModel và RelayCommand.

🛠 Công nghệ sử dụng
Ngôn ngữ: C# (.NET 9.0)

Giao diện: WPF, MaterialDesignThemes

Xử lý dữ liệu: System.Text.Json (JSON), ExcelDataReader (Import Excel)

📋 Hướng dẫn sử dụng
Chạy ứng dụng: Mở tệp .sln bằng Visual Studio và chạy dự án ASM_PRN212_BL3.

Đăng nhập:

Chọn Admin để truy cập các tính năng quản lý.

Chọn Student để bắt đầu quá trình ôn luyện.

Dữ liệu: Dữ liệu được lưu trữ tự động trong các tệp data.json và users.json tại thư mục output sau khi build.
