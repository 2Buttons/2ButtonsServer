using System;
using System.Threading.Tasks;
using AuthorizationData.Account.DTO;
using AuthorizationData.Account.Entities;
using CommonLibraries;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationData.Account.Repostirories
{
  public class SocialRepository : IDisposable
  {
    private readonly TwoButtonsAccountContext _contextAccount;

    public SocialRepository(TwoButtonsAccountContext context)
    {
      _contextAccount = context;
    }

    public void Dispose()
    {
      _contextAccount.Dispose();
    }

   
  }
}