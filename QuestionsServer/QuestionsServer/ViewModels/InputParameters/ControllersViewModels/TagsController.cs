﻿using System.Collections.Generic;

namespace QuestionsServer.ViewModels.InputParameters.ControllersViewModels
{
  public class AddTags
  {
    public List<string> Tags { get; set; }
  }

  public class GetTags
  {
    public PageParams PageParams { get; set; } = new PageParams();
  }
}