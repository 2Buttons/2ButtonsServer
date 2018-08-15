using CommonLibraries;

namespace QuestionsData.DTO
{
  public class AuthorDto
  {
    public int UserId { get; set; }
    public string Login { get; set; }
    public SexType SexType { get; set; }
    public string OriginalAvatarUrl { get; set; }
  }
}