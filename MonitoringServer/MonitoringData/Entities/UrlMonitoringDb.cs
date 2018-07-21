using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitoringData.Entities
{
  [Table("UrlMonitorings")]
  public class UrlMonitoringDb
  {
    [Key]
    public int UrlMonitoringId { get; set; }

    public int UserId { get; set; }
    public int GetsQuestionsUserAsked { get; set; } = 0;
    public int GetsQuestionsUserAnswered { get; set; } = 0;
    public int GetsQuestionsUserFavorite { get; set; } = 0;
    public int GetsQuestionsUserCommented { get; set; } = 0;
    public int GetsQuestionsPersonalAsked { get; set; } = 0;
    public int GetsQuestionsPersonalRecommended { get; set; } = 0;
    public int GetsQuestionsPersonalSelected { get; set; } = 0;
    public int GetsQuestionsPersonalLiked { get; set; } = 0;
    public int GetsQuestionsPersonalSaved { get; set; } = 0;
    public int GetsQuestionsPersonalDayTop { get; set; } = 0;
    public int GetsQuestionsPersonalWeekTop { get; set; } = 0;
    public int GetsQuestionsPersonalMonthTop { get; set; } = 0;
    public int GetsQuestionsPersonalAllTimeTop { get; set; } = 0;
    public int GetsQuestionsNews { get; set; } = 0;
    public int OpensPersonalPage { get; set; } = 0;
    public int OpensUserPage { get; set; } = 0;
    public int GetsNotifications { get; set; } = 0;
    public int FiltersQuestions { get; set; } = 0;
    public int OpensQuestionPage { get; set; } = 0;
    public int GetsComments { get; set; } = 0;
  }
}