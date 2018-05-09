using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TwoButtonsAccountDatabase.Entities;

namespace TwoButtonsAccountDatabase
{
  public class TokenRepository : IDisposable
  {
    private readonly TwoButtonsAccountContext _context;


    public TokenRepository(TwoButtonsAccountContext context)
    {
      _context = context;
    }


    public void Dispose()
    {
      _context.Dispose();
    }

    public async Task<bool> AddToken(TokenDb token)
    {
      var existingToken =
        _context.Tokens.SingleOrDefault(r => r.UserId == token.UserId && r.ClientId == token.ClientId);

      if (existingToken != null)
        await RemoveToken(existingToken);

      _context.Tokens.Add(token);

      return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveToken(int tokenId)
    {
      var refreshToken = await _context.Tokens.FindAsync(tokenId);

      if (refreshToken == null) return false;

      _context.Tokens.Remove(refreshToken);
      return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveToken(TokenDb refreshToken)
    {
      _context.Tokens.Remove(refreshToken);
      return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveTokens(IEnumerable<TokenDb> tokens)
    {
     _context.Tokens.RemoveRange(tokens);
      return await _context.SaveChangesAsync() >= tokens.Count();
    }


    public async Task<TokenDb> FindToken(string tokenId)
    {
      return await _context.Tokens.FindAsync(tokenId);
    }


    public TokenDb GetToken(int clientId, string refreshToken)
    {
      return _context.Tokens.FirstOrDefault(x => x.ClientId == clientId && x.RefreshToken == refreshToken);
    }

    public List<TokenDb> GetAllTokens()
    {
      return _context.Tokens.ToList();
    }
  }
}