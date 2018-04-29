using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountServer.Repositories
{
    public class AccountManager
    {
    private AccountContext _context;

      public AccountManager(AccountContext context)
      {
        _context = context;
      }
  }
}
