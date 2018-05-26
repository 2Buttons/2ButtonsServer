using System.ComponentModel.DataAnnotations;

namespace QuestionsData.Entities.Moderators
{
    public class ComplainDb
    {
        [Key]
        public int ComplainId { get; set; }
        public int QuestionId { get; set; }
        public int ComplainAmount { get; set; }
    }
}
