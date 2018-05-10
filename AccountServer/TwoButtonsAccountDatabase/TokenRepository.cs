using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

    public async Task<bool> AddTokenAsync(TokenDb token)
    {
      var existingToken =
        _context.Tokens.SingleOrDefault(r => r.UserId == token.UserId && r.ClientId == token.ClientId);

      if (existingToken != null)
        await RemoveTokenAsync(existingToken);

      _context.Tokens.Add(token);

      return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveTokenAsync(int tokenId)
    {
      var refreshToken = await _context.Tokens.FindAsync(tokenId);

      if (refreshToken == null) return false;

      _context.Tokens.Remove(refreshToken);
      return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveTokenAsync(TokenDb refreshToken)
    {
      _context.Tokens.Remove(refreshToken);
      return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveTokensAsync(IEnumerable<TokenDb> tokens)
    {
     _context.Tokens.RemoveRange(tokens);
      return await _context.SaveChangesAsync() >= tokens.Count();
    }

    public async Task<bool> RemoveTokensAsync(Expression<Func<TokenDb, bool>>predicate)
    {
      var tokensForRemoving = _context.Tokens.AsNoTracking().Where(predicate);
      _context.Tokens.RemoveRange(tokensForRemoving);
      return await _context.SaveChangesAsync() >= tokensForRemoving.Count();
    }


    public async Task<TokenDb> FindTokenAsync(string tokenId)
    {
      return await _context.Tokens.FindAsync(tokenId);
    }


    public async Task<TokenDb> FindTokenAsync(int clientId, string token)
    {
      return await _context.Tokens.AsNoTracking().FirstOrDefaultAsync(x => x.ClientId == clientId && x.RefreshToken == token);
    }

    public async Task<List<TokenDb>> GetAllTokensAsync()
    {
      return await _context.Tokens.AsNoTracking().ToListAsync();
    }
  }
}