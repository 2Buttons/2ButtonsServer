using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountServer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using System.Web;
using Microsoft.EntityFrameworkCore;

namespace AccountServer.Repositories
{
  public class AuthenticationRepository : IDisposable
  {
    private readonly AccountContext _context;


    public AuthenticationRepository(AccountContext context)
    {
      _context = context;
    }

    public async Task<bool> AddClient(Client client)
    {
      await _context.Clients.AddAsync(client);
      return await _context.SaveChangesAsync()>0;
    }

    public async Task<bool> UpdateClient(Client client)
    {
      _context.Entry(client).State = EntityState.Modified;
      return await _context.SaveChangesAsync() > 0;
    }


    public bool TryFindClient(int clientId, string secret, out Client client)
    {
      if (clientId !=0 && !string.IsNullOrEmpty(secret))
      {
        client = _context.Clients.SingleOrDefault(x=>x.ClientId==clientId && x.Secret == secret);
        return true;
      }

      client = new Client();
      return false;
    }

    public async Task<bool> AddToken(Token token)
    {

      var existingToken = _context.RefreshTokens.SingleOrDefault(r => r.UserId == token.UserId && r.ClientId == token.ClientId);

      if (existingToken != null)
      {
        await RemoveToken(existingToken);
      }

      _context.RefreshTokens.Add(token);

      return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveToken(string refreshTokenId)
    {
      var refreshToken = await _context.RefreshTokens.FindAsync(refreshTokenId);

      if (refreshToken == null) return false;

      _context.RefreshTokens.Remove(refreshToken);
      return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveToken(Token refreshToken)
    {
      _context.RefreshTokens.Remove(refreshToken);
      return await _context.SaveChangesAsync() > 0;
    }


    public async Task<Token> FindToken(string refreshTokenId)
    {
      var refreshToken = await _context.RefreshTokens.FindAsync(refreshTokenId);

      return refreshToken;
    }


    public Token GetToken(int clientId, int userId, string refreshToken)
    {
      return _context.RefreshTokens.FirstOrDefault(x => x.ClientId == clientId && x.UserId == userId &&
                                                        x.RefreshToken == refreshToken);
    }

    public List<Token> GetAllTokens()
    {
      return _context.RefreshTokens.ToList();
    }


    public void Dispose()
    {
      _context.Dispose();

    }


  }
}
