﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AuthorizationData.Account.DTO;
using AuthorizationData.Account.Entities;
using AuthorizationData.Account.Entities.FunctionEntities;
using AuthorizationData.Main;
using CommonLibraries;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationData.Account.Repostirories
{
  public class UserRepository : IDisposable
  {
    private readonly TwoButtonsAccountContext _contextAccount;
    private readonly TwoButtonsContext _contextMain;

    public UserRepository(TwoButtonsContext contextMain, TwoButtonsAccountContext context)
    {
      _contextAccount = context;
      _contextMain = contextMain;
    }

    public void Dispose()
    {
      _contextAccount.Dispose();
      _contextMain.Dispose();
    }

    public async Task<bool> AddUserIntoAccountDbAsync(UserDb user)
    {
      await _contextAccount.UsersDb.AddAsync(user);
      return await _contextAccount.SaveChangesAsync() > 0;
    }

    public async Task<bool> AddUserIntoMainDbAsync(int userId, string login, DateTime birthDate, SexType sex,
      string city, string description, string fullAvatarLink, string smallAvatarLink)
    {
      var returnsCode = new SqlParameter
      {
        SqlDbType = SqlDbType.Int,
        Direction = ParameterDirection.Output
      };

      try
      {
        return await _contextMain.Database.ExecuteSqlCommandAsync(
                 $"addUser {userId}, {login}, {birthDate}, {sex}, {city},  {description}, {fullAvatarLink}, {smallAvatarLink}, {returnsCode} OUT") >
               0;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      return false;
    }

    public async Task<bool> AddOrChangeExternalUserIdAsync(int userId, long externalUserId, SocialNetType socialType,
      string externalToken)
    {
      var user = await _contextAccount.UsersDb.FindAsync(userId);
      if (user == null) return false;
      switch (socialType)
      {
        case SocialNetType.Facebook:
          user.FacebookId = externalUserId;
          user.FacebookToken = externalToken;
          break;
        case SocialNetType.Vk:
          user.VkId = (int)externalUserId;
          user.VkToken = externalToken;
          break;
        case SocialNetType.Nothing:
        case SocialNetType.Twiter:
        case SocialNetType.GooglePlus:
        case SocialNetType.Telegram:
        case SocialNetType.Badoo:
        default:
          return false;
      }

      _contextAccount.Entry(user).State = EntityState.Modified;
      return await _contextAccount.SaveChangesAsync() > 0;
    }


    public async Task<bool> ChangeUserRoleAsync(int userId, RoleType role)
    {
      var user = await _contextAccount.UsersDb.FindAsync(userId);
      if (user == null) return false;

      user.RoleType = role;
      _contextAccount.Entry(user).State = EntityState.Modified;
      return await _contextAccount.SaveChangesAsync() > 0;
    }


    public async Task<bool> ChangeUserEmail(int userId, string email, bool emailConfirmed)
    {
      var user = await _contextAccount.UsersDb.FindAsync(userId);
      if (user == null) return false;

      user.Email = email;
      user.EmailConfirmed = emailConfirmed;
      _contextAccount.Entry(user).State = EntityState.Modified;
      return await _contextAccount.SaveChangesAsync() > 0;
    }

    public async Task<bool> ChangeUserPasswordAsync(int userId, string oldPasswordHash, string newPasswordHash)
    {
      var user = await _contextAccount.UsersDb.FindAsync(userId);
      if (user == null) return false;
      if (user.PasswordHash != oldPasswordHash) return false;

      user.PasswordHash = newPasswordHash;
      _contextAccount.Entry(user).State = EntityState.Modified;
      return await _contextAccount.SaveChangesAsync() > 0;
    }

    public async Task<UserDto> GetUserByEmail(string email)
    {
      var user = await _contextAccount.UsersDb.AsNoTracking()
        .FirstOrDefaultAsync(x => x.Email == email);
      return user?.ToUserDto();
    }

    public async Task<UserDto> GetUserByUserId(int userId)
    {
      var user = await _contextAccount.UsersDb.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId);
      return user?.ToUserDto();
    }

    public List<UserIdDb> GetUserIdFromVkId(IEnumerable<int> ids)
    {
      var dataTable = new DataTable();
      dataTable.Columns.Add("id", typeof(int));
      foreach (var id in ids)
        dataTable.Rows.Add(id);
      var vkIdTable = new SqlParameter
      {
        ParameterName = "@VkIDTable",
        TypeName = "dbo.idTable",
        Value = dataTable
      };
      return _contextAccount.UserIds.FromSql($"select * from dbo.getUserIdFromVkId(@VkIDTable)", vkIdTable)
        .ToList();
    }

    public async Task<UserDto> GetUserByEmailAndPasswordAsync(string email, string passwordHash)
    {
      var user = await _contextAccount.UsersDb.AsNoTracking()
        .FirstOrDefaultAsync(x => x.Email == email && x.PasswordHash == passwordHash);
      return user?.ToUserDto();
    }

    public async Task<UserDto> GetUserByPhoneAndPasswordAsync(string phone, string passwordHash)
    {
      var user = await _contextAccount.UsersDb.AsNoTracking()
        .FirstOrDefaultAsync(x => x.PhoneNumber == phone && x.PasswordHash == passwordHash);
      return user?.ToUserDto();
    }

    public async Task<UserDto> GetUserByExternalUserIdAsync(long externalUserId, SocialNetType socialType)
    {
      UserDb user;
      switch (socialType)
      {
        case SocialNetType.Facebook:
          user = await _contextAccount.UsersDb.AsNoTracking().FirstOrDefaultAsync(x => x.FacebookId == externalUserId);
          break;
        case SocialNetType.Vk:
          user = await _contextAccount.UsersDb.AsNoTracking().FirstOrDefaultAsync(x => x.VkId == externalUserId);
          break;
        case SocialNetType.Nothing:
        case SocialNetType.Twiter:
        case SocialNetType.GooglePlus:
        case SocialNetType.Telegram:
        case SocialNetType.Badoo:
        default:
          user = null;
          break;
      }
      return user?.ToUserDto();
    }

    public async Task<long> GetExternalUserIdAsync(int userId, SocialNetType socialType)
    {
      var user = await _contextAccount.UsersDb.FindAsync(userId);
      if (user == null) return 0;
      switch (socialType)
      {
        case SocialNetType.Facebook:
          return user.FacebookId;
        case SocialNetType.Vk:
          return user.VkId;
        case SocialNetType.Twiter:
        case SocialNetType.GooglePlus:
        case SocialNetType.Telegram:
        case SocialNetType.Badoo:
        case SocialNetType.Nothing:
        default:
          return 0;
      }
    }

    public async Task<bool> IsUserIdExistAsync(int userId)
    {
      return await _contextAccount.UsersDb.FindAsync(userId) != null;
    }

    public async Task<bool> IsUserExistByPhoneAsync(string phone)
    {
      return await _contextAccount.UsersDb.AsNoTracking().AnyAsync(x => x.PhoneNumber == phone);
    }

    public async Task<bool> IsUserExistByEmailAsync(string email)
    {
      return await _contextAccount.UsersDb.AsNoTracking().AnyAsync(x => x.Email == email);
    }

    public async Task<RoleType> GetUserRoleAsync(int userId)
    {
      var user = await _contextAccount.UsersDb.FindAsync(userId);
      if (user != null)
        return user.RoleType;
      return RoleType.Guest;
    }


    public async Task<UserContactsDto> GetUserSocialsAsync(int userId)
    {
      var user = await _contextAccount.UsersDb.FindAsync(userId);
      if (user == null) return new UserContactsDto();

      return new UserContactsDto
      {
        VkId = user.VkId,
        FacebookId = user.FacebookId
      };
    }

    public async Task<bool> RemoveUserAsync(int userId)
    {
      var user = await _contextAccount.UsersDb.FindAsync(userId);
      if (user == null) return false;
      _contextAccount.UsersDb.Remove(user);
      return await _contextAccount.SaveChangesAsync() > 0;
    }
  }
}