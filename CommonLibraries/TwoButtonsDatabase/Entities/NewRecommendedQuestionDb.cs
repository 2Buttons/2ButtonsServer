using System;
using System.ComponentModel.DataAnnotations;

namespace TwoButtonsDatabase.Entities
{
    public partial class NewRecommendedQuestionDb
    {
        [Key]
        public int UserId { get; set; }
        public bool Recommended { get; set; }
        public DateTime QuestionAddDate { get; set; }
        public string Condition { get; set; }
        public string Login { get; set; }
        public string SmallAvatarLink { get; set; }
    }
}
