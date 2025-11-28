README: MINI ORDER MANAGEMENT API
Tổng quan dự án
- Dự án Mini Order Management API là một hệ thống quản lý đơn hàng đơn giản, được phát triển bằng ASP.NET Core 8 (Backend) và giao diện người dùng cơ bản bằng HTML/JS thuần (Frontend).
Yêu cầu Hệ thống.
- NET 8 SDK (Hoặc phiên bản mới hơn)
- Visual Studio Code (Hoặc Visual Studio 2022)
- IIS (Internet Information Services) đã được cài đặt và kích hoạt .NET 8 Hosting Bundle trên Windows.
I. Cấu hình & Khởi chạy Backend (API)
- Backend được triển khai trên IIS để đảm bảo môi trường Production.
1. Cấu hình Database & Publish
- Dọn dẹp File cũ: Xóa file shopdemo.db trong thư mục Publish (D:\MyWeb\MiniOrderBE) nếu bạn đã chạy lần trước.
- Mở Terminal trong thư mục gốc của dự án Backend (BaitapcuoikyBE).
- Chạy lệnh Publish để biên dịch và đưa code mới nhất (đã Fix lỗi Foreign Key, JSON Cycle) sang thư mục IIS: Bashdotnet publish -c Release -o D:\MyWeb\MiniOrderBE
Khởi động lại IIS: Sau khi Publish, mở IIS Manager và Restart website MiniOrderAPI (Port 8090).
2. Kiểm tra APITruy cập: http://localhost:8090/swagger
- Kết quả: Giao diện Swagger UI sẽ hiện ra, xác nhận API đã hoạt động.
II. Cấu hình & Khởi chạy Frontend (Giao diện)
- Frontend là các file tĩnh (HTML/CSS/JS) được cấu hình chạy trên Site riêng biệt (Port 8091).
1. Cập nhật File Mới
- Đảm bảo bạn đã copy toàn bộ file HTML/CSS/JS (đã có Menu điều hướng hoàn chỉnh) vào thư mục Frontend trên IIS (mặc định: D:\MyWeb\MiniOrderFE).
2. Truy cập Ứng dụng
- Truy cập trực tiếp vào trang đăng nhập qua IIS: hhttp://localhost:8091/login.html
III. Tài khoản Mặc định
- Sử dụng các tài khoản sau để kiểm tra phân quyền (đã được tạo sẵn trong DbSeeder):
Trang mặc định sau Login: Adminadmin@shop.testAdmin@123 có quyền: Quản lý Sản phẩm (products.html)
                          Useruser@shop.testUser@123 có quyền: Trang Mua hàng (create-order.html)
IV. Kiểm tra Chức năng Đã Fix
- Sử dụng các tài khoản trên để kiểm tra các lỗi đã được khắc phục:
- Cập nhật Sản phẩm: Login Admin --> Thử Sửa một sản phẩm (Xác nhận lỗi 405 Method Not Allowed đã bị loại bỏ).
- Đặt hàng: Login User --> Thêm vào giỏ --> Xác nhận đặt hàng (Xác nhận lỗi Foreign Key đã được sửa).
- Điều hướng: Menu chuyển đổi giữa các chức năng (Sản phẩm <--> Khách hàng <--> Đơn hàng) hoạt động đúng theo Role.
