using System.ComponentModel.DataAnnotations;

namespace TwoButtonsDatabase.Entities.Moderators
{
    public class ComplaintDb
    {
        [Key]
        public int ComplaintId { get; set; }
        public int QuestionId { get; set; }
        public int ComplaintAmount { get; set; }
    }
}
