using System;
using System.ComponentModel.DataAnnotations;

namespace TwoButtonsDatabase.Entities
{
    public partial class UserAskedQuestionsDb
    {
        [Key]
        public int QuestionId { get; set; }
        public string Condition { get; set; }
        public string FirstOption { get; set; }
        public string SecondOption { get; set; }
        public int? QuestionType { get; set; }
        public DateTime Asked { get; set; }
        public int? UserId { get; set; }
        public string Login { get; set; }
        public string AvatarLink { get; set; }
        public int? Answers { get; set; }
        public int? Likes { get; set; }
        public int? Dislikes { get; set; }
        public int? YourFeedback { get; set; }
        public int? Answered { get; set; }
        public int? Comments { get; set; }
    }
}
