using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace BotsData
{
  public class TwoButtonsContext : DbContext
  {

    public DbSet<QuestionEntity> QuestionEntities { get; set; }
    public DbSet<OptionEntity> OptionEntities { get; set; }

    public TwoButtonsContext(DbContextOptions<TwoButtonsContext> options) : base(options)
    {
    }
  }
}
