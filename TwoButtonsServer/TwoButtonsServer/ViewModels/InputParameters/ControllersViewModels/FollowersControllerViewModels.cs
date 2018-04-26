namespace TwoButtonsServer.ViewModels.InputParameters.ControllersViewModels
{
    public class FollowerViewModel : UserPageIdViewModel
    {
        public int Amount { get; set; } = 100;
        public string SearchedLogin { get; set; } = "";
    }
}
