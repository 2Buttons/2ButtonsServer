using System.Collections.Generic;

namespace CommonLibraries
{
  public class ServersSettings
  {
    public List<Server> Servers { get; set; }

    public Server this[string name]
    {
      get
      {
        var index = Servers.FindIndex(x => x.Name == name);
        return index < 0 ? null : Servers[index];
      }
      set
      {
        var index = Servers.FindIndex(x => x.Name == name);
        if (index < 0) return;
        Servers[index] = value;
      }
    }
  }

  public class Server
  {
    public string Name { get; set; }
    public int Port { get; set; }
  }
}