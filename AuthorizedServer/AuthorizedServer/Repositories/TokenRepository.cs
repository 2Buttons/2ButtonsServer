using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizedServer.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        public bool AddToken(RToken token)
        {
            using (DemoDbContext db = new DemoDbContext())
            {
                db.RTokens.Add(token);

                return db.SaveChanges() > 0;
            }
        }

        public bool ExpireToken(RToken token)
        {
            using (DemoDbContext db = new DemoDbContext())
            {
                db.RTokens.Update(token);

                return db.SaveChanges() > 0;
            }
        }

        public RToken GetToken(string refresh_token, string client_id)
        {
            using (DemoDbContext db = new DemoDbContext())
            {
                return db.RTokens.FirstOrDefault(x => x.ClientId == client_id && x.RefreshToken == refresh_token);
            }
        }
    }
}
