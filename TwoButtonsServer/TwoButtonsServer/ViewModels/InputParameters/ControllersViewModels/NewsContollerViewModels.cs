namespace TwoButtonsServer.ViewModels.InputParameters.ControllersViewModels
{
    public class GetNewsViewModel : UserIdViewModel
    {
        public PageParams PageParams { get; set; } = new PageParams();
    }
}
