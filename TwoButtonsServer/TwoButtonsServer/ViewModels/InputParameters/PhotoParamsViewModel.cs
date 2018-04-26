namespace TwoButtonsServer.ViewModels.InputParameters
{
    public class PhotoParamsViewModel
    {
        public int PhotosAmount { get; set; } = 100;

        public int MinAge { get; set; } = 0;

        public int MaxAge { get; set; } = 100;

        public int Sex { get; set; } = 0;

        public string City { get; set; } = string.Empty;
    }
}