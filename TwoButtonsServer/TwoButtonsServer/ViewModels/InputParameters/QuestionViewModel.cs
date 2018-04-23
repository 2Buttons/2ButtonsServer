namespace TwoButtonsServer.ViewModels.InputParameters
{
    public class QuestionViewModel
    {
        public int UserId { get; set; }
        public string Condition { get; set; }
        public int Anonymity { get; set; }
        public int Audience { get; set; }
        public int QuestionType { get; set; }
        public int StandartPictureId { get; set; }
        public string FirstOption { get; set; }
        public string SecondOption { get; set; }
        public string BackgroundImageLink { get; set; } = null;
    }
}