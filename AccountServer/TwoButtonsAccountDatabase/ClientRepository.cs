using System;
using System.Linq;
using System.Threading.Tasks;
using AccountServer.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace TwoButtonsAccountDatabase
{
  public class ClientRepository : IDisposable
  {
    private readonly TwoButtonsAccountContext _context;


    public ClientRepository(TwoButtonsAccountContext context)
    {
      _context = context;
    }

    public void Dispose()
    {
      _context.Dispose();
    }

    public async Task<bool> AddClientAsync(ClientDb client)
    {
      _context.Clients.Add(client);
      return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateClientAsync(ClientDb client)
    {
      _context.Entry(client).State = EntityState.Modified;
      return await _context.SaveChangesAsync() > 0;
    }


    public async Task<ClientDb> FindClientAsync(int clientId, string secret)
    {
      var client =  await _context.Clients.FindAsync(clientId);
      return client.Secret == secret ? client : null;
    }

    public async Task<bool> RemoveClientAsync(int clientId, string secret)
    {
      var client = await FindClientAsync(clientId, secret);
      if (client == null)
        return false;
      _context.Clients.Remove(client);
      return await _context.SaveChangesAsync() > 0;
    }
  }
}