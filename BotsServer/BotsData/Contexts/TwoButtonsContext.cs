using CommonLibraries.Entities.Main;
using Microsoft.EntityFrameworkCore;

namespace BotsData.Contexts
{
  public class TwoButtonsContext : DbContext
  {
    public DbSet<QuestionEntity> QuestionEntities { get; set; }
    public DbSet<OptionEntity> OptionEntities { get; set; }
    public DbSet<AnswerEntity> AnswerEntities { get; set; }

    public TwoButtonsContext(DbContextOptions<TwoButtonsContext> options) : base(options)
    {
    }
  }
}