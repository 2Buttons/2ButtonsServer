using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonLibraries.CommandLine
{
  public class CommandLinePattern
  {
    public string Name { get; set; }
    public List<string> Parameters { get; set; }
    public List<(string name, string verbose)> Options { get; set; }

    public CommandLinePattern HasOption(string name, string verboseName = null)
    {
      Options.Add((name, verboseName));

      return this;
    }

    public CommandLinePattern HasParameter(string name)
    {
      Parameters.Add(name);

      return this;
    }

    public virtual bool TryParse(string[] args, out Command result)
    {
      result = null;

      if (args.Length == 0) return false;
      if (args[0] != Name.ToLower()) return false;

      var properties = new Dictionary<string, string>();
      var nextParameterIndex = 0;

      foreach (var t in args)
        if (t.StartsWith("-"))
        {
          var optionObject = t.Split(':');
          var optionName = optionObject[0];
          if (!Options.Any(x => x.name == optionName || x.verbose == optionName)) return false;

          properties.Add(optionName.Replace("-",""), optionName.Length == 1 ? null : optionObject[1]);
        }
        else if(Parameters.Count>0)
        {
          var name = Parameters[nextParameterIndex];
          nextParameterIndex++;

          var value = t;
          properties.Add(name, value);
        }
      // Для команды с имененем Help мы найдём класс HelpCommand:
      var className = Name.Replace(Name[0].ToString(), Name[0].ToString().ToUpper()) + "Command";
      var type = Type.GetType("CommonLibraries.CommandLine."+className, true);
      // И создадим его экземпляр:
      result = (Command) Activator.CreateInstance(type);
      // Теперь значения всех параметров запишем в свойства
      // только что созданного экземляра:
      foreach (var property in properties)
      {
        var name = property.Key.Replace(property.Key[0].ToString(), property.Key[0].ToString().ToUpper());
        var value = property.Value;

        type.GetProperty(name).SetValue(result, value);
      }
      return true;
    }

    public virtual Command Parse(string[] args)
    {
      Command result;
      if (TryParse(args, out result)) return result;

      throw new FormatException();
    }

    public static CommandLinePattern operator |(CommandLinePattern left, CommandLinePattern right)
    {
      return new OrCommandLinePattern(left, right);
    }
  }
}