using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommonLibraries;
using Microsoft.EntityFrameworkCore;
using TwoButtonsAccountDatabase.DTO;
using TwoButtonsAccountDatabase.Entities;

namespace TwoButtonsAccountDatabase
{
  public class UserRepository : IDisposable
  {
    private readonly TwoButtonsAccountContext _context;

    public UserRepository(TwoButtonsAccountContext context)
    {
      _context = context;
    }

    public void Dispose()
    {
      _context.Dispose();
    }

    public async Task<bool> AddUserAsync(UserDb user)
    {
      await _context.Users.AddAsync(user);
      return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> AddOrChangeExternalUserIdAsync(int userId, int externalUserId, SocialNetType socialType,
      string externalToken)
    {
      var user = await _context.Users.FindAsync(userId);
      if (user == null) return false;
      switch (socialType)
      {
        case SocialNetType.Facebook:
          user.FacebookId = externalUserId;
          user.FacebookToken = externalToken;
          break;
        case SocialNetType.Vk:
          user.VkId = externalUserId;
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

      _context.Entry(user).State = EntityState.Modified;
      return await _context.SaveChangesAsync() > 0;
    }


    public async Task<bool> ChangeUserRoleAsync(int userId, RoleType role)
    {
      var user = await _context.Users.FindAsync(userId);
      if (user == null) return false;

      user.RoleType = role;
      _context.Entry(user).State = EntityState.Modified;
      return await _context.SaveChangesAsync() > 0;
    }


    public async Task<bool> ChangeUserEmail(int userId, string email, bool emailConfirmed)
    {
      var user = await _context.Users.FindAsync(userId);
      if (user == null) return false;

      user.Email = email;
      user.EmailConfirmed = emailConfirmed;
      _context.Entry(user).State = EntityState.Modified;
      return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> ChangeUserPasswordAsync(int userId, string oldPasswordHash, string newPasswordHash)
    {
      var user = await _context.Users.FindAsync(userId);
      if (user == null) return false;
      if (user.PasswordHash != oldPasswordHash) return false;

      user.PasswordHash = newPasswordHash;
      _context.Entry(user).State = EntityState.Modified;
      return await _context.SaveChangesAsync() > 0;
    }

    public async Task<UserDto> GetUserByEmail(string email)
    {
      var user =   await _context.Users.AsNoTracking()
        .FirstOrDefaultAsync(x => x.Email == email);
      return user?.ToUserDto();

    }

    public async Task<UserDto> GetUserByUserId(int userId)
    {
      var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x=>x.UserId == userId);
      return user?.ToUserDto();

    }

    public async Task<UserDto> GetUserByEmailAndPasswordAsync(string email, string passwordHash)
    {
      var user = await _context.Users.AsNoTracking()
        .FirstOrDefaultAsync(x => x.Email == email && x.PasswordHash == passwordHash);
      return user?.ToUserDto();
    }

    public async Task<UserDto> GetUserByPhoneAndPasswordAsync(string phone, string passwordHash)
    {
      var user = await _context.Users.AsNoTracking()
        .FirstOrDefaultAsync(x => x.PhoneNumber == phone && x.PasswordHash == passwordHash);
      return user?.ToUserDto();
    }

    public async Task<UserDto> GetUserByExternalUserIdAsync(int externalUserId, SocialNetType socialType)
    {
      UserDb user;
      switch (socialType)
      {
        case SocialNetType.Facebook:
          user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.FacebookId == externalUserId);
          break;
        case SocialNetType.Vk:
          user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.VkId == externalUserId);
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
      var user = await _context.Users.FindAsync(userId);
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
      return await _context.Users.FindAsync(userId) != null;
    }

    public async Task<bool> IsUserExistByPhoneAsync(string phone)
    {
      return await _context.Users.AsNoTracking().AnyAsync(x => x.PhoneNumber == phone);
    }

    public async Task<bool> IsUserExistByEmailAsync(string email)
    {
      return await _context.Users.AsNoTracking().AnyAsync(x=>x.Email == email);
    }

    public async Task<RoleType> GetUserRoleAsync(int userId)
    {
      var user = await _context.Users.FindAsync(userId);
      if (user != null)
        return user.RoleType;
      return RoleType.Guest;
    }


    public async Task<UserContactsDto> GetUserSocialsAsync(int userId)
    {
      var user = await _context.Users.FindAsync(userId);
      if (user == null) return new UserContactsDto();

      return new UserContactsDto
      {
        VkId = user.VkId,
        FacebookId = user.FacebookId
      };
    }

    public async Task<bool> RemoveUserAsync(int userId)
    {
      var user = await _context.Users.FindAsync(userId);
      if (user == null) return false;
      _context.Users.Remove(user);
      return await _context.SaveChangesAsync() > 0;
    }
  }
}