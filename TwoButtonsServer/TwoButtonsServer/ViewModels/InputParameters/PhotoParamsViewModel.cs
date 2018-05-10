using CommonLibraries;
using TwoButtonsServer.ViewModels.OutputParameters;

namespace TwoButtonsServer.ViewModels.InputParameters
{
    public class PhotoParamsViewModel
    {
        public int PhotosAmount { get; set; } = 100;

        public int MinAge { get; set; } = 0;

        public int MaxAge { get; set; } = 100;

        public SexType SexType { get; set; } = 0;

        public string City { get; set; } = string.Empty;
    }
}