using System;
using System.Collections.Generic;
using CommonLibraries;
using QuestionsData.Queries;

namespace QuestionsData.DTO
{
  public class QiestionStatisticUsersDto
  {
    public List<List<VoterDto>> Voters = new List<List<VoterDto>>();
  }

  public class UsersInfoForQuestionStatisticDto
  {
    public List<VoterDto> Users { get; set; } = new List<VoterDto>();
  }

  public class VoterDto
  {
    public int UserId { get; set; }
    public string Login { get; set; }
    public SexType Sex { get; set; }
    public int Age { get; set; }
    public string SmallAvatarLink { get; set; }
    public bool IsYouFollowed { get; set; }
    public bool IsHeFollowed { get; set; }
    public string City { get; set; }
  }
}