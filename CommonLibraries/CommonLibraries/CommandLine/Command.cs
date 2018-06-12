using System.Collections.Generic;

namespace CommonLibraries.CommandLine
{
  public class Command: ICommand
  {
    public static CommandLinePattern WithName(string name)
    {
      return new CommandLinePattern {Name = name, Parameters = new List<string>(), Options = new List<(string, string)>()};
    }

    public static string GetPort(string[] args)
    {
      if (args.Length == 0) return "";

      if (args[0].StartsWith("--"))
      {
        var parameterWithoutHyphen = args[0].Substring(2);
        var nameValue = parameterWithoutHyphen.Split(':');
        return nameValue[1];
      }
      if (args[0].StartsWith("-"))
      {
        var parameterWithoutHyphen = args[0].Substring(1);
        var nameValue = parameterWithoutHyphen.Split(':');
        return nameValue[1];
      }
      return "";
    }

    public void Run()
    {
      throw new System.NotImplementedException();
    }
  }
}