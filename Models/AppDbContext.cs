using Microsoft.EntityFrameworkCore;

namespace VkBot.Models
{
    public class AppDbContext : DbContext

    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserState> UserStates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseNpgsql("Host=localhost;Database=VkBotDb;Username=postgres;Password=password;");
        // TODO: вписать реальные данные для подключения к БД 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Конфигурация моделей (если нужно)
            base.OnModelCreating(modelBuilder);
        }
    }

}
