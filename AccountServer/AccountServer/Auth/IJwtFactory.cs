using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AccountServer.Helpers;
using CommonLibraries;

namespace AccountServer.Auth
{
    public interface IJwtFactory
    {
      Task<string> GenerateEncodedAccessToken(int userId, RoleType role);
      Task<ClaimsIdentity> GenerateAccessClaimsIdentity(int userId, RoleType role);

      Task<string> GenerateEncodedRefreshToken(int userId);
      Task<ClaimsIdentity> GenerateRefreshClaimsIdentity(int userId);
  }
}
