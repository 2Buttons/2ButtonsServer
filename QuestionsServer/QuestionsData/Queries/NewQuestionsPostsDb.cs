using System;
using System.ComponentModel.DataAnnotations;

namespace QuestionsData.Queries
{
    public partial class NewQuestionsPostsDb
    {
        [Key]
        public int QuestionId { get; set; }
        public string Condition { get; set; }
        public int QuestionType { get; set; }
        public DateTime QuestionAddDate { get; set; }
        public int AskerId { get; set; }
        public string Login { get; set; }
        public string OriginalAvatarUrl { get; set; }
        public int Answers { get; set; }
        public int Raiting { get; set; }
        public int YourFeedback { get; set; }
    }
}
