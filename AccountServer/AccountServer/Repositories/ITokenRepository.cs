using AccountServer.Models;

namespace AccountServer.Repositories
{
    public interface ITokenRepository
    {
        bool AddToken(Token token);

        bool ExpireToken(Token token);

        Token GetToken(string userId, string refreshToken);
    }
}
