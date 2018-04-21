using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorizedServer.Models;

namespace AuthorizedServer.Repositories
{
    public interface ITokenRepository
    {
        bool AddToken(Token token);

        bool ExpireToken(Token token);

        Token GetToken(string refresh_token, string client_id);
    }
}
