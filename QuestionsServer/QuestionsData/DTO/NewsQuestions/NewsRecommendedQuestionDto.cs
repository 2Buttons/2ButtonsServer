using QuestionsData.Queries.NewsQuestions;

namespace QuestionsData.DTO.NewsQuestions
{
    public class NewsRecommendedQuestionDto : NewsQuestionBaseDto
    {
        public NewsRecommendedQuestionDb NewsRecommendedQuestionDb { get; set; }
    }
}