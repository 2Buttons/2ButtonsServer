using System;
using CommonLibraries;

namespace DataGenerator.Data.Reader.Objects
{
  public class User
  {
    public int UserId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public SexType Sex { get; set; }

    public DateTime Birthday { get; set; }

    public City City { get; set; }

    public string SmallPhoto { get; set; }

    public string LargePhoto { get; set; }
  }
}