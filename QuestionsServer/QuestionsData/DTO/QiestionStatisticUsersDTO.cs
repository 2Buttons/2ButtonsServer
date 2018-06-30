using System;
using System.Collections.Generic;
using CommonLibraries;
using QuestionsData.Queries;

namespace QuestionsData.DTO
{
  public class QiestionStatisticUsersDto
  {
    public List<List<VoterUserDto>> Voters = new List<List<VoterUserDto>>();
  }

  public class UsersInfoForQuestionStatisticDto
  {
    public List<VoterUserDto> Users { get; set; } = new List<VoterUserDto>();
  }

  public class VoterUserDto
  {
    public int UserId { get; set; }
    public string Login { get; set; }
    public SexType SexType { get; set; }
    public int Age { get; set; }
    public string SmallAvatarLink { get; set; }
    public bool IsYouFollowed { get; set; }
    public bool IsHeFollowed { get; set; }
    public string City { get; set; }
  }
}