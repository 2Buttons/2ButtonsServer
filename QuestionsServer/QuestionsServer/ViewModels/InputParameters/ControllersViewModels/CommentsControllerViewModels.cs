using CommonLibraries;

namespace QuestionsServer.ViewModels.InputParameters.ControllersViewModels
{

    public class AddCommentViewModel : QuestionIdViewModel
    {
        public string CommentText { get; set; }
        public int PreviousCommnetId { get; set; } = 0;

    }

    public class AddCommentFeedbackViewModel : UserIdViewModel
    {
        public int CommentId { get; set; }
        public FeedbackType FeedbackType { get; set; }

    }

    public class GetCommentsViewModel : QuestionIdViewModel
    {
        public int Amount { get; set; } = 10000;

    }
}
