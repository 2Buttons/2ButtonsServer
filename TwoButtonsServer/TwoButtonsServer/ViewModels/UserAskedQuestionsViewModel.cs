using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TwoButtonsServer.ViewModels
{
    public partial class UserAskedQuestionsViewModel
    {
        public int QuestionId { get; set; }
        public string Condition { get; set; }
        public string FirstOption { get; set; }
        public string SecondOption { get; set; }
        public string BackgroundImageLink { get; set; }
        public int? QuestionType { get; set; }
        public DateTime QuestionAddDate { get; set; }
        public int? UserId { get; set; }
        public string Login { get; set; }
        public string SmallAvatarLink { get; set; }
        public int? Answers { get; set; }
        public int? Likes { get; set; }
        public int? Dislikes { get; set; }
        public int? YourFeedback { get; set; }
        public int? YourAnswer { get; set; }
        public int? InFavorites { get; set; }
        public int? Comments { get; set; }

        public List<TagViewModel> Tags { get; set; }
    }
}
