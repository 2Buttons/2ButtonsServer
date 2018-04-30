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
        public string Text { get; set; }
        public int LikesAmount { get; set; }
        public int DislikesAmount { get; set; }
        public FeedbackType YourFeedbackType { get; set; }
        public int PreviousCommentId { get; set; }
        public DateTime AddDate { get; set; }
    }
}
