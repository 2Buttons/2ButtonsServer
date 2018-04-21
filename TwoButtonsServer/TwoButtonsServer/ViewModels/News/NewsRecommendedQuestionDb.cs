﻿using System;
using TwoButtonsDatabase.Entities;

namespace TwoButtonsServer.ViewModels.News
{
    public class NewsRecommendedQuestionViewModel : QuestionBaseViewModel
    {
        public int RecommendedUserId { get; set; }
        public string RecommendedUserLogin { get; set; }
        public DateTime RecommendedDate { get; set; }
    }
}