using System;
using System.Collections;
using System.Collections.Generic;
using CommonLibraries;
using Newtonsoft.Json;

namespace QuestionsServer.ViewModels.OutputParameters.NewsQuestions
{
  public class NewsQuestionBaseViewModel : QuestionBaseViewModel, IEquatable<NewsQuestionBaseViewModel>, IEqualityComparer<NewsQuestionBaseViewModel>
  {
    public SexType Sex { get; set; }
    public int Position { get; set; }

    [JsonIgnore]
    public NewsType Type { get; set; }
    [JsonIgnore]
    public int Priority { get; set; }

    public new bool Equals(object obj1, object obj2)
    {
      QuestionBaseViewModel x = (QuestionBaseViewModel)obj1;
      QuestionBaseViewModel y = (QuestionBaseViewModel)obj2;
      return x.Condition == y.Condition && x.UserId == y.UserId;

    }

    public bool Equals(NewsQuestionBaseViewModel other)
    {

      //Check whether the compared object is null. 
      if (other == null) return false;

      //Check whether the compared object references the same data. 
      if (ReferenceEquals(this, other)) return true;

      //Check whether the products' properties are equal.
      return Condition == other.Condition && UserId == other.UserId;
    }

    // If Equals() returns true for a pair of objects  
    // then GetHashCode() must return the same value for these objects. 

    public override int GetHashCode()
    {

      //Get hash code for the Name field if it is not null. 
      int hashProductName = Condition == null ? 0 : Condition.GetHashCode();

      //Get hash code for the Code field. 
      int hashProductCode = Options.GetHashCode();

      //Calculate the hash code for the product. 
      return hashProductName ^ hashProductCode ^ UserId;
    }

    public bool Equals(NewsQuestionBaseViewModel x, NewsQuestionBaseViewModel y)
    {
      throw new NotImplementedException();
    }

    public int GetHashCode(NewsQuestionBaseViewModel obj)
    {
      throw new NotImplementedException();
    }
  }
}

