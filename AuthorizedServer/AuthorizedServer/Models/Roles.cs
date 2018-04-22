using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationServer.Models
{
    public class Roles
    {
        public const string UnauthorizedUser = "UnauthorizedUser";
        public const string AuthorizedUser = "AuthorizedUser";
        public const string Moderator = "Moderator";
        public const string Administrator = "Administrator";
    }
}
