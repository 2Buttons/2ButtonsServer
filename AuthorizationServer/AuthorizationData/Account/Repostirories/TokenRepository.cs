using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

    public async Task<bool> AddTokenAsync(TokenEntity token)
    {
      var existingToken =
        _context.TokenEntities.SingleOrDefault(r => r.TokenId == token.TokenId);

      if (existingToken != null)
        await RemoveTokenAsync(existingToken);

      _context.TokenEntities.Add(token);

      return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveTokenAsync(long tokenId)
    {
      var refreshToken = await _context.TokenEntities.FindAsync(tokenId);

      if (refreshToken == null) return false;

      _context.TokenEntities.Remove(refreshToken);
      return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveTokenAsync(TokenEntity refreshToken)
    {
      _context.TokenEntities.Remove(refreshToken);
      return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveTokensAsync(IEnumerable<TokenEntity> tokens)
    {
     _context.TokenEntities.RemoveRange(tokens);
      return await _context.SaveChangesAsync() >= tokens.Count();
    }

    public async Task<bool> RemoveTokensAsync(Expression<Func<TokenEntity, bool>>predicate)
    {
      var tokensForRemoving = _context.TokenEntities.AsNoTracking().Where(predicate);
      _context.TokenEntities.RemoveRange(tokensForRemoving);
      return await _context.SaveChangesAsync() >= tokensForRemoving.Count();
    }

    public async Task<bool> RemoveTokensByUserIdAsync(int userId)
    {
      var tokensForRemoving = _context.TokenEntities.AsNoTracking().Where(x => x.UserId == userId).ToList();
      var tokensCount = tokensForRemoving.Count();
      _context.TokenEntities.RemoveRange(tokensForRemoving);
      return await _context.SaveChangesAsync() >=  tokensCount;
    }


    public async Task<TokenEntity> FindTokenByIdAsync(int tokenId)
    {
      return await _context.TokenEntities.FindAsync(tokenId);
    }

    public async Task<TokenEntity> FindTokenByTokenAndUserIdAsync(int userId, string token)
    {
      return await _context.TokenEntities.SingleOrDefaultAsync(x=>x.UserId == userId && x.RefreshToken == token);
    }


    public async Task<TokenEntity> FindTokenAsync(string token)
    {
      return await _context.TokenEntities.AsNoTracking().FirstOrDefaultAsync(x=>x.RefreshToken == token);
    }

    public async Task<int> CountTokensForUserAsync(int userId)
    {
      return await _context.TokenEntities.CountAsync(x=>x.UserId == userId);
    }

    public async Task<List<TokenEntity>> GetAllTokensAsync()
    {
      return await _context.TokenEntities.AsNoTracking().ToListAsync();
    }
  }
}