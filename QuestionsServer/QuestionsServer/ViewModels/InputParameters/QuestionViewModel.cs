namespace QuestionsServer.ViewModels.InputParameters
{
    public class QuestionViewModel : QuestionIdViewModel
    {
        public PageParams PageParams { get; set; } = new PageParams();
    }
}
