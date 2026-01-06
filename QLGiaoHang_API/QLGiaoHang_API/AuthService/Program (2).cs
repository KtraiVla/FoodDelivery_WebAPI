using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace API_Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Nạp cấu hình từ file ocelot.json
            // (Nếu file json lỗi hoặc không có, dòng này sẽ báo lỗi khi chạy)
            builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // 2. Đăng ký dịch vụ Ocelot
            builder.Services.AddOcelot(builder.Configuration);

            var app = builder.Build();

            // Gateway thường không cần Swagger UI, nhưng giữ lại cũng không sao
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            // 3. Kích hoạt Ocelot (QUAN TRỌNG: Phải dùng .Wait() vì đây là hàm void Main)
            app.UseOcelot().Wait();

            app.Run();
        }
    }
}