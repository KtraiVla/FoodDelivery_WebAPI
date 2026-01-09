using AuthService.Interfaces;
using AuthService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Shared.Helpers;
using System.Text;

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
            // đăng ký AccountService
            builder.Services.AddScoped<IAccountService, AuthService.Services.AccountService>();

            // ====================================================
            // 3. CẤU HÌNH JWT (Để dùng được [Authorize])
            // ====================================================
            var jwtSettings = builder.Configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
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
