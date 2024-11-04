using Microsoft.EntityFrameworkCore; // Nhập không gian tên cho Entity Framework Core
using ChatBotAPI.Data; // Nhập không gian tên cho dữ liệu của ứng dụng

var builder = WebApplication.CreateBuilder(args); // Tạo builder cho ứng dụng web

// Cấu hình DbContext với SQL Server
builder.Services.AddDbContext<ChatBotDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ChatBotDb"))); // Thiết lập DbContext để sử dụng SQL Server với chuỗi kết nối từ cấu hình

// Thêm dịch vụ controller
builder.Services.AddControllers(); // Thêm dịch vụ cho các controller

// Thêm Swagger
builder.Services.AddEndpointsApiExplorer(); // Cung cấp khả năng khám phá các endpoint API
builder.Services.AddSwaggerGen(); // Thêm Swagger để tạo tài liệu cho API

var app = builder.Build(); // Xây dựng ứng dụng web

// Cấu hình Swagger
if (app.Environment.IsDevelopment()) // Kiểm tra nếu đang ở môi trường phát triển
{
    app.UseSwagger(); // Kích hoạt Swagger
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ChatBotAPI V1"); // Đặt endpoint cho Swagger
        // c.RoutePrefix = string.Empty; // Đặt Swagger làm trang mặc định (bình luận lại)
    });
}

// Cấu hình middleware
app.UseAuthorization(); // Kích hoạt middleware cho xác thực
app.MapControllers(); // Ánh xạ các controller vào ứng dụng

// Chạy ứng dụng
app.Run(); // Bắt đầu chạy ứng dụng
