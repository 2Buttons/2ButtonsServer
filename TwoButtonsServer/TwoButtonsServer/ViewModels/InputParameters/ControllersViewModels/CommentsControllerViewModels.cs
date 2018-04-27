namespace TwoButtonsServer.ViewModels.InputParameters.ControllersViewModels
{

    public class AddCommentViewModel : QuestionIdViewModel
    {
        public string CommentText { get; set; }
        public int PreviousCommnetId { get; set; } = 0;

    }

    public class AddCommentFeedbackViewModel : UserIdViewModel
    {
        public int CommentId { get; set; }
        public int Feedback { get; set; }

    }

    public class GetCommentsViewModel : QuestionIdViewModel
    {
        public int CommentsAmount { get; set; } = 10000;

    }
}
