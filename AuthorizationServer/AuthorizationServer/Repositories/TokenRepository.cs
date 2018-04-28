using System.Linq;
using AuthorizationServer.Models;

namespace AuthorizationServer.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        public bool AddToken(Token token)
        {
            using (DemoDbContext db = new DemoDbContext())
            {
                db.Tokens.Add(token);

                return db.SaveChanges() > 0;
            }
        }

        public bool ExpireToken(Token token)
        {
            using (DemoDbContext db = new DemoDbContext())
            {
                db.Tokens.Update(token);

                return db.SaveChanges() > 0;
            }
        }

        public Token GetToken(string userId, string refreshToken)
        {
            using (DemoDbContext db = new DemoDbContext())
            {
                return db.Tokens.FirstOrDefault(x => x.UserId == userId && x.RefreshToken == refreshToken);
            }
        }
    }
}
