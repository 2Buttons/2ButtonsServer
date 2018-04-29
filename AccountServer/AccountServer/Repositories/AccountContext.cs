using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AccountServer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AccountServer.Repositories
{
    public class AccountContext:IdentityDbContext<IdentityUser>
    {
    public AccountContext(DbContextOptions<AccountContext> options)
      : base(options)
    {
    }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //  {
    //    var connStr = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AuthenticationDatabase.db");

    //    optionsBuilder.UseSqlite($"Data Source={connStr}");
    //  }

    public DbSet<Client> Clients { get; set; }
      public DbSet<RefreshToken> RefreshTokens { get; set; }


    }

}
