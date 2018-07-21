using CommonLibraries;

namespace QuestionsServer.ViewModels.InputParameters
{
    public class PhotoParamsViewModel
    {
        public int PhotosCount { get; set; } = 100;

        public int MinAge { get; set; } = 0;

        public int MaxAge { get; set; } = 100;

        public SexType SexType { get; set; } = 0;

        public string City { get; set; } = string.Empty;
    }
}