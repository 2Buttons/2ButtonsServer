using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataGenerator.ScriptsGenerators.FunctionInsertion.Queries;

namespace DataGenerator.ScriptsGenerators.FunctionInsertion.ScriptGenerators
{
  public class TagGenerator
  {
    private readonly string _db = "TwoButtons";

    private string GetUsingDb()
    {
      return $"USE [{_db}]";
    }

    private string GetGo()
    {
      return Environment.NewLine + "GO" + Environment.NewLine;
    }

    private string GetInsertionTagLine(TagQuery tag)
    {
      return
        $"EXECUTE [dbo].[addTag] {tag.QuestionId}, N'{tag.Text}', {tag.Position}";
    }

    private string GetInsertionTagsLine(IList<TagQuery> tags)
    {
      var result = new StringBuilder();
      for (var i = 0; i < tags.Count - 1; i++)
        result.Append(GetInsertionTagLine(tags[i]) + ";" + Environment.NewLine);
      result.Append(GetInsertionTagLine(tags[tags.Count - 1]));
      return result.ToString();
    }

    public string GetInsertionLine(IList<TagQuery> tags)
    {
      var result = new StringBuilder();
      var times = tags.Count < 1000 ? 1 : tags.Count / 1000;
      for (var i = 0; i < times; i++)
      {
        var tagsIter = tags.Skip(i * 1000).Take(1000).ToList();
        result.Append(GetUsingDb());
        result.Append(GetGo());
        result.Append(GetInsertionTagsLine(tagsIter));
        result.Append(GetGo());
      }
      return result.ToString();
    }
  }
}