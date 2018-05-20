﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace TwoButtonsAccountDatabase.Helpers
{
    public static class ParametersToTable
    {
      public static DataTable ToTableValuedParameter<T, TProperty>(this IEnumerable<T> list, Func<T, TProperty> selector)
      {
        var tbl = new DataTable();
        tbl.Columns.Add("Id", typeof(T));

        foreach (var item in list)
        {
          tbl.Rows.Add(selector.Invoke(item));

        }

        return tbl;

      }
  }
}
