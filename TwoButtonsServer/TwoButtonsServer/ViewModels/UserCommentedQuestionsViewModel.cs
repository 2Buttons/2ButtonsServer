﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TwoButtonsServer.ViewModels
{
    public partial class UserCommentedQuestionsViewModel
    {
        public int QuestionId { get; set; }
        public string Condition { get; set; }
        public string FirstOption { get; set; }
        public string SecondOption { get; set; }
        public int? QuestionType { get; set; }
        public DateTime QuestionAddDate { get; set; }
        public int? UserId { get; set; }
        public string Login { get; set; }
        public string AvatarLink { get; set; }
        public int? Answers { get; set; }
        public int? Likes { get; set; }
        public int? Dislikes { get; set; }
        public int? YourFeedback { get; set; }
        public int? YourAnswer { get; set; }
        public int? InFavorites { get; set; }
        public int? Comments { get; set; }

        public int? CommentId { get; set; }
        public string Comment { get; set; }
        public int? CommentLikes { get; set; }
        public int? CommentDislikes { get; set; }
        public int? YourCommentFeedback { get; set; }
        public int? PreviousCommentId { get; set; }
        public DateTime CommentAddDate { get; set; }

        public List<TagViewModel> Tags { get; set; }
    }
}