using System.ComponentModel.DataAnnotations;
using CommonLibraries;

namespace QuestionsData.Queries.Moderators
{
    public class ComplaintDb
    {
        [Key]
        public int QuestionId { get; set; }
        public ComplaintType ComplaintType { get; set; }
        public int ComplaintsCount { get; set; }
    }
}
