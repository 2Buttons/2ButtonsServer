﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountServer.ViewModels.OutputParameters.User
{
  public class UserStatisticsViewModel
  {
    public int PublicAskedQuestions { get; set; }
    public int AskedQuestions { get; set; }
    public int AnsweredQuestions { get; set; }
    public int SeenQuestions { get; set; }
    public int PublicFavoriteQuestions { get; set; }
    public int FavoriteQuestions { get; set; }
    public int CommentsWritten { get; set; }

    public int UserQuestionsShows { get; set; }
    public int UserQuestionsAnswers { get; set; }

    public int QuestionsCommentsGot { get; set; }
    public int QuestionsLikesGot { get; set; }
    public int QuestionsDislikesGot { get; set; }
    public int CommentsLikesGot { get; set; }
    public int CommentsDislikesGot { get; set; }
    public int QuestionsLikesMade { get; set; }
    public int QuestionsDislikesMade { get; set; }
    public int CommentsLikesMade { get; set; }
    public int CommentsDislikesMade { get; set; }
  }
}
