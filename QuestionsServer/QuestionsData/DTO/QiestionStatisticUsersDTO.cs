using System.Collections.Generic;
using QuestionsData.Queries;

namespace QuestionsData.DTO
{
  public class QiestionStatisticUsersDto
  {
    public List<List<PhotoDb>> Voters = new List<List<PhotoDb>>();
  }

  public class UsersInfoForQuestionStatisticDto
  {
    public List<PhotoDb> Users { get; set; } = new List<PhotoDb>();
  }
}