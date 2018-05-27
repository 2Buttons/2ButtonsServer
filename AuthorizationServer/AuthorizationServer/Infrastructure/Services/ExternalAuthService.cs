using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthorizationData;
using AuthorizationData.Account.DTO;
using AuthorizationData.Account.Entities;
using AuthorizationData.Main.Entities;
using AuthorizationServer.Models;
using AuthorizationServer.Services;
using AuthorizationServer.ViewModels.InputParameters;
using AuthorizationServer.ViewModels.InputParameters.Auth;
using CommonLibraries;
using CommonLibraries.Extensions;
using CommonLibraries.Response;

namespace AuthorizationServer.Infrastructure.Services
{
  public class ExternalAuthService : IDisposable
  {
    private readonly AuthorizationUnitOfWork _db;
    private readonly IJwtService _jwtService;

    public ExternalAuthService(IJwtService jwtService, AuthorizationUnitOfWork db)
    {
      _db = db;
      _jwtService = jwtService;
    }

    public void Dispose()
    {
      _db.Dispose();
    }
  }
}
