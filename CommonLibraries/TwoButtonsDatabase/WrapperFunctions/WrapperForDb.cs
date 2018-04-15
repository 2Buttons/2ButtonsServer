using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace TwoButtonsDatabase.WrapperFunctions
{
    public abstract class BaseWrapperForDb
    {
        public readonly TwoButtonsContext Db;

        protected BaseWrapperForDb(TwoButtonsContext dbContext)
        {
            Db = dbContext;
        }
    }
}
