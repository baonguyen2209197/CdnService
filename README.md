# CdnService

## Mô tả
Dự án ASP.NET Core cung cấp API để upload và truy xuất ảnh từ thư mục cấu hình.

## Cấu hình
- Thư mục lưu ảnh được cấu hình qua file `appsettings.json` với key `ImageFolder`.
- Nếu không cấu hình, mặc định là thư mục `Images` trong thư mục gốc dự án.

Ví dụ cấu hình:
```json
{
  "ImageFolder": "Images"
}
```

## Sử dụng DI cho cấu hình
- Class `ImageFolderOptions` dùng để ánh xạ cấu hình.
- Đăng ký cấu hình trong `Program.cs`:
  ```csharp
  builder.Services.Configure<ImageFolderOptions>(builder.Configuration);
  ```
- Controller inject `IOptions<ImageFolderOptions>` để lấy giá trị thư mục ảnh.

## API
### 1. Lấy ảnh
- `GET /api/image/{filename}`
- Trả về file ảnh nếu tồn tại, trả về 404 nếu không tìm thấy.

### 2. Upload nhiều ảnh
- `POST /api/image/upload`
- Body: Form-data, key `files` (có thể upload nhiều file cùng lúc)
- Giới hạn: tối đa 20 file/lượt, mỗi file tối đa 8MB, chỉ nhận `.jpg`, `.jpeg`, `.png`, `.gif`
- Trả về số lượng file upload thành công.

## Lưu ý
- Đường dẫn thư mục ảnh luôn là absolute path để tránh lỗi khi truy xuất file.
- Không thao tác trực tiếp với file txt cho cấu hình, chỉ dùng DI.

## Chạy ứng dụng
1. Cài đặt .NET 8 SDK
2. Chạy lệnh:
   ```sh
   dotnet run
   ```
3. Truy cập Swagger UI tại `https://localhost:5001/swagger` để thử API.

## Thư mục
- `Controllers/ImageController.cs`: Controller chính xử lý API ảnh
- `ImageFolderOptions.cs`: Class ánh xạ cấu hình thư mục ảnh
- `Images/`: Thư mục chứa ảnh
- `appsettings.json`: File cấu hình

## Liên hệ
- Tác giả: [Tên của bạn]
