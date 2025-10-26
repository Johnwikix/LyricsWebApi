using LyricsWeb.Model;
using Microsoft.EntityFrameworkCore;

namespace LyricsWeb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // 为 TodoItem 模型定义 DbSet，它对应数据库中的一个表
        public DbSet<SongItem> SongItems { get; set; }

        // 如果您使用的是 ASP.NET Core 的较旧版本或特殊配置，
        // 可能需要在 Program.cs/Startup.cs 中配置连接字符串
        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     optionsBuilder.UseSqlite("Data Source=MyLocalDatabase.db");
        // }
    }
}
