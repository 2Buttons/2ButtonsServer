using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AuthorizedServer.Models
{
    public class DemoDbContext : DbContext
    {
        public DbSet<Token> Tokens { get; set; }

        //public DbSet<AToken> ATokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connStr = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TokensDatabase.db");

            optionsBuilder.UseSqlite($"Data Source={connStr}");
        }
    }
}
