using CommonLibraries;

namespace TwoButtonsServer.ViewModels.OutputParameters
{
    public class PhotoViewModel
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public SexType SexType { get; set; }
        public int Age { get; set; }
        public string SmallAvatarLink { get; set; }
    }
}