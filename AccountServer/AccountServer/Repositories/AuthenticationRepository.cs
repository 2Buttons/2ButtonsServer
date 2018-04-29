using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountServer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace AccountServer.Repositories
{
    public class AuthenticationRepository
    {
      private AccountContext _context;


      public AuthenticationRepository(AccountContext context)
      {
        _context = context;
      }

      //public async Task<IdentityResult> RegisterUser(UserModel userModel)
      //{
      //  IdentityUser user = new IdentityUser
      //  {
      //    UserName = userModel.UserName
      //  };

      //  var result = await _userManager.CreateAsync(user, userModel.Password);

      //  return result;
      //}

      //public async Task<IdentityUser> FindUser(string userName, string password)
      //{
      //  IdentityUser user = await _userManager.FindAsync(userName, password);

      //  return user;
      //}

      public Client FindClient(string clientId)
      {
        var client = _context.Clients.Find(clientId);

        return client;
      }

      public async Task<bool> AddRefreshToken(RefreshToken token)
      {

        var existingToken = _context.RefreshTokens.Where(r => r.Subject == token.Subject && r.ClientId == token.ClientId).SingleOrDefault();

        if (existingToken != null)
        {
           await RemoveRefreshToken(existingToken);
        }

        _context.RefreshTokens.Add(token);

        return await _context.SaveChangesAsync() > 0;
      }

      public async Task<bool> RemoveRefreshToken(string refreshTokenId)
      {
        var refreshToken = await _context.RefreshTokens.FindAsync(refreshTokenId);

        if (refreshToken == null) return false;

        _context.RefreshTokens.Remove(refreshToken);
        return await _context.SaveChangesAsync() > 0;
      }

      public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
      {
        _context.RefreshTokens.Remove(refreshToken);
        return await _context.SaveChangesAsync() > 0;
      }

      public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
      {
        var refreshToken = await _context.RefreshTokens.FindAsync(refreshTokenId);

        return refreshToken;
      }

      public List<RefreshToken> GetAllRefreshTokens()
      {
        return _context.RefreshTokens.ToList();
      }

      //public async Task<IdentityUser> FindAsync(UserLoginInfo loginInfo)
      //{
      //  IdentityUser user = await _userManager.FindAsync(loginInfo);

      //  return user;
      //}

      //public async Task<IdentityResult> CreateAsync(IdentityUser user)
      //{
      //  var result = await _userManager.CreateAsync(user);

      //  return result;
      //}

      //public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
      //{
      //  var result = await _userManager.AddLoginAsync(userId, login);

      //  return result;
      //}

      public void Dispose()
      {
        _context.Dispose();

      }
  }
}
