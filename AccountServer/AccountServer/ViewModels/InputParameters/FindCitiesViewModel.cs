namespace AccountServer.ViewModels.InputParameters
{
  public class FindCitiesViewModel
  {
    public string Pattern { get; set; }
    public PageParams PageParams { get; set; } = new PageParams();
  }
}