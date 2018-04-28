using System;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationServer.Models
{
    public class DemoDbContext : DbContext
    {
        public DbSet<Token> Tokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connStr = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TokensDatabase.db");

            optionsBuilder.UseSqlite($"Data Source={connStr}");
        }
    }
}
