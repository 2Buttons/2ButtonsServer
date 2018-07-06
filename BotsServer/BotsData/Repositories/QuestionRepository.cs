using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotsData.Repositories
{
    class QuestionRepository
    {

      private readonly TwoButtonsContext _context;
    public QuestionRepository(TwoButtonsContext context)
      {
        _context = context;
      }

      public Task<QuestionEntity> GetQuestions(int offset, int count)
      {
        _context.QuestionEntities.Where()
      }

  }
}
