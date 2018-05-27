using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AuthorizationData.Account;
using AuthorizationData.Account.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationData.Account.Repostirories
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
        _context.TokensDb.SingleOrDefault(r => r.TokenId == token.TokenId);

      if (existingToken != null)
        await RemoveTokenAsync(existingToken);

      _context.TokensDb.Add(token);

      return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveTokenAsync(int tokenId)
    {
      var refreshToken = await _context.TokensDb.FindAsync(tokenId);

      if (refreshToken == null) return false;

      _context.TokensDb.Remove(refreshToken);
      return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveTokenAsync(TokenDb refreshToken)
    {
      _context.TokensDb.Remove(refreshToken);
      return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveTokensAsync(IEnumerable<TokenDb> tokens)
    {
     _context.TokensDb.RemoveRange(tokens);
      return await _context.SaveChangesAsync() >= tokens.Count();
    }

    public async Task<bool> RemoveTokensAsync(Expression<Func<TokenDb, bool>>predicate)
    {
      var tokensForRemoving = _context.TokensDb.AsNoTracking().Where(predicate);
      _context.TokensDb.RemoveRange(tokensForRemoving);
      return await _context.SaveChangesAsync() >= tokensForRemoving.Count();
    }

    public async Task<bool> RemoveTokensByUserIdAsync(int userId)
    {
      var tokensForRemoving = _context.TokensDb.AsNoTracking().Where(x => x.UserId == userId).ToList();
      var tokensCount = tokensForRemoving.Count();
      _context.TokensDb.RemoveRange(tokensForRemoving);
      return await _context.SaveChangesAsync() >=  tokensCount;
    }


    public async Task<TokenDb> FindTokenByIdAsync(int tokenId)
    {
      return await _context.TokensDb.FindAsync(tokenId);
    }

    public async Task<TokenDb> FindTokenByTokenAndUserIdAsync(int userId, string token)
    {
      return await _context.TokensDb.SingleOrDefaultAsync(x=>x.UserId == userId && x.RefreshToken == token);
    }


    public async Task<TokenDb> FindTokenAsync(string token)
    {
      return await _context.TokensDb.AsNoTracking().FirstOrDefaultAsync(x=>x.RefreshToken == token);
    }

    public async Task<int> CountTokensForUserAsync(int userId)
    {
      return await _context.TokensDb.CountAsync(x=>x.UserId == userId);
    }

    public async Task<List<TokenDb>> GetAllTokensAsync()
    {
      return await _context.TokensDb.AsNoTracking().ToListAsync();
    }
  }
}