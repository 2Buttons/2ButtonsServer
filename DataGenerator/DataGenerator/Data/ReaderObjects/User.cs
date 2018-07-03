using System;
using CommonLibraries;

namespace DataGenerator.Data.ReaderObjects
{
  public class User
  {
    public int UserId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public SexType Sex { get; set; }

    public DateTime Birthday { get; set; }

    public int CityId { get; set; }

    public string SmallPhoto { get; set; }

    public string LargePhoto { get; set; }
  }
}