using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwoButtonsServer.ViewModels.OutputParameters
{
    public class CommentViewModel
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public string Login { get; set; }
        public string SmallAvatarLink { get; set; }
        public string CommentText { get; set; }
        public int CommentLikesAmount { get; set; }
        public int CommentDislikesAmount { get; set; }
        public FeedbackType YourFeedbackType { get; set; }
        public int PreviousCommentId { get; set; }
        public DateTime CommentAddDate { get; set; }
    }
}
