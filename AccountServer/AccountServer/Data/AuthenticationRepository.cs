﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountServer.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountServer.Data
{
  public class AuthenticationRepository : IDisposable
  {
    private readonly TwoButtonsAccountContext _context;


    public AuthenticationRepository(TwoButtonsAccountContext context)
    {
      _context = context;
    }


    public void Dispose()
    {
      _context.Dispose();
    }

    public async Task<bool> AddClient(ClientDb client)
    {
      await _context.Clients.AddAsync(client);
      return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateClient(ClientDb client)
    {
      _context.Entry(client).State = EntityState.Modified;
      return await _context.SaveChangesAsync() > 0;
    }


    public bool TryFindClient(int clientId, string secret, out ClientDb client)
    {
      if (clientId != 0 && !string.IsNullOrEmpty(secret))
      {
        client = _context.Clients.SingleOrDefault(x => x.ClientId == clientId && x.Secret == secret);
        return true;
      }

      client = new ClientDb();
      return false;
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
      return await _context.SaveChangesAsync() > 0;
    }


    public async Task<TokenDb> FindToken(string refreshTokenId)
    {
      var refreshToken = await _context.Tokens.FindAsync(refreshTokenId);

      return refreshToken;
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