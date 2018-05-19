using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AccountServer.Helpers;
using AccountServer.Models;
using AccountServer.ViewModels.OutputParameters;
using CommonLibraries;
using Microsoft.IdentityModel.Tokens;

namespace AccountServer.Services
{
  public interface IJwtService
  {
    Task<Token> GenerateJwtAsync(int userId, RoleType role);
    //Task<string> GenerateEncodedAccessToken(int userId, RoleType role);
    //Task<string> GenerateEncodedRefreshToken(int userId, RoleType role);
    //Task<ClaimsIdentity> GenerateClaimsIdentity(int userId, RoleType role);
  }
}
