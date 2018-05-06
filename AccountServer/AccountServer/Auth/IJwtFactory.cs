using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AccountServer.Helpers;

namespace AccountServer.Auth
{
    public interface IJwtFactory
    {
      Task<string> GenerateEncodedToken(int userId, RoleType role);
      Task<ClaimsIdentity> GenerateClaimsIdentity(int userId, RoleType role);
  }
}
