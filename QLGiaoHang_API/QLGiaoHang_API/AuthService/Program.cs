using AuthService.Interfaces;
using AuthService.Services;
using Shared.Helpers;

namespace AuthService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // lấy chuỗi kết nối từ appsettings.json
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            // đăng ký SqlHelper
            builder.Services.AddScoped<SqlHelper>(provider => new SqlHelper(connectionString));

            // đăng ký AuthService
            builder.Services.AddScoped<IAuthService, AuthService.Services.AuthService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
