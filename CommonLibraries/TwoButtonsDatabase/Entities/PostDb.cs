using System;
using System.ComponentModel.DataAnnotations;

namespace TwoButtonsDatabase.Entities
{
    public partial class PostDb
    {
        [Key]
        public int QuestionId { get; set; }
        public string Condition { get; set; }
        public string FirstOption { get; set; }
        public string SecondOption { get; set; }
        public int? QuestionType { get; set; }
        public DateTime QuestionAddDate { get; set; }
        public string Login { get; set; }
        public string AvatarLink { get; set; }
        public int? LoginUserId { get; set; }
        public int? AvatarUserId { get; set; }
        public int? Anwsers { get; set; }
        public int? Raiting { get; set; }
        public int? YourFeedback { get; set; }
        public int? InFavorites { get; set; }
        public int? Anwsered { get; set; }
        public DateTime Posted { get; set; }
    }
}
