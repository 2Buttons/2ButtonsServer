namespace AccountServer.ViewModels.InputParameters
{
  public class GetRecommendedUsers : UserIdViewModel
  {
    public PageParams PageParams = new PageParams();
  }

  public class PageParams
  {
    public int Offset { get; set; } = 0;
    public int Count { get; set; } = 5;
  }
}