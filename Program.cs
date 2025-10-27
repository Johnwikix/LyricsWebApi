using LyricsWeb.Data;
using LyricsWeb.Data.Service;
using LyricsWeb.Service;
using Microsoft.EntityFrameworkCore;

namespace LyricsWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));
            builder.Services.AddControllers();
            builder.Services.AddScoped<ILyricsCoverService, LyricsCoverService>();
            builder.Services.AddScoped<IDataService, DataService>();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }
            app.UseHttpsRedirection();
            app.UseAuthorization();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    context.Database.Migrate();
                }
                catch 
                {
                }
            }

            app.MapControllers();

            app.Run();
        }
    }
}
