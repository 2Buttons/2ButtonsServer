using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibraries;

namespace QuestionsServer.ViewModels.OutputParameters
{
    public class AuthorViewModel
    {
      public int UserId { get; set; }
      public string Login { get; set; }
      public SexType SexType { get; set; }
      public string SmallAvatarUrl { get; set; }
  }
}
