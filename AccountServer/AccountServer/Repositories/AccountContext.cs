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
    public class AccountContext:DbContext
    {
    public AccountContext(DbContextOptions<AccountContext> options)
      : base(options)
    {
    }

    public DbSet<Client> Clients { get; set; }
      public DbSet<Token> RefreshTokens { get; set; }


    }

}
