using System;
using CommonLibraries;

namespace DataGenerator.Data.Reader.Objects
{
  public class UserReader
  {
    public int ExternalId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public SexType SexType { get; set; }
    public DateTime Birthday { get; set; }
    public string City { get; set; }
    public string VkOriginalAvatrUrl { get; set; }
  }
}