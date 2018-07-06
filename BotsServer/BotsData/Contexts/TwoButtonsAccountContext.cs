using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace BotsData
{
  public class TwoButtonsAccountContext : DbContext
  {

    public DbSet<UserEntity> UserEntities { get; set; }

    public TwoButtonsAccountContext(DbContextOptions<TwoButtonsAccountContext> options) : base(options)
    {
    }
  }
}
