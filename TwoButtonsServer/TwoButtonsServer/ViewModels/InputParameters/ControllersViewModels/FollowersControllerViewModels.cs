namespace TwoButtonsServer.ViewModels.InputParameters.ControllersViewModels
{
    public class FollowerViewModel : UserPageIdViewModel
    {
        public PageParams PageParams { get; set; } = new PageParams();
        public string SearchedLogin { get; set; } = "";
    }
}
