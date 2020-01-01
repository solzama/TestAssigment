using GameEngine;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;

namespace DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<GameSettings> GameSettingses { get; set; }= default!;
        
        private static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder => { builder.AddConsole(); });

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameSettings>(entity =>
            {
                entity.HasKey(g => g.GameSettingsId);
                entity.Property(g => g.GameName)
                    .HasDefaultValue("Connect 4");
                entity.Property(g => g.BoardHeight)
                    .HasDefaultValue(6);
                entity.Property(g => g.BoardWidth)
                    .HasDefaultValue(7);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLoggerFactory(MyLoggerFactory)
                .UseSqlite("Data Source=C:/Users/Daria/RiderProjects/charp2019fall/Connect4/connect4.db");
        }
    }
}