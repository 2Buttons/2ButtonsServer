using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibraries;

namespace MediaServer.Models
{
  public class SizedUrl<T> where T : struct
  {
    public T Size { get; set; }
    public string Url { get; set; }
  }
}
