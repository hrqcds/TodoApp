using Microsoft.EntityFrameworkCore;
using TodoApp.Data;

namespace TodoApp.Config
{
    public class ConfigBuilder
    {
        public static WebApplication Run()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<AppContextDb>(
                options => options.UseSqlite(
                    builder.Configuration.GetConnectionString("DefaultConnection")));
            return builder.Build();
        }
    }
}