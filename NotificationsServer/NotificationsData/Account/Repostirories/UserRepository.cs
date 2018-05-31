using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NotificationsData.Account.Entities;
using NotificationsData.DTO;

namespace NotificationsData.Account.Repostirories
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


    public async Task<bool> ChangeUserRoleAsync(int userId, RoleType role)
    {
      var user = await _context.UsersDb.FindAsync(userId);
      if (user == null) return false;

      user.RoleType = role;
      _context.Entry(user).State = EntityState.Modified;
      return await _context.SaveChangesAsync() > 0;
    }


    public async Task<bool> ChangeUserEmail(int userId, string email, bool emailConfirmed)
    {
      var user = await _context.UsersDb.FindAsync(userId);
      if (user == null) return false;

      user.Email = email;
      user.EmailConfirmed = emailConfirmed;
      _context.Entry(user).State = EntityState.Modified;
      return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> ChangeUserPasswordAsync(int userId, string oldPasswordHash, string newPasswordHash)
    {
      var user = await _context.UsersDb.FindAsync(userId);
      if (user == null) return false;
      if (user.PasswordHash != oldPasswordHash) return false;

      user.PasswordHash = newPasswordHash;
      _context.Entry(user).State = EntityState.Modified;
      return await _context.SaveChangesAsync() > 0;
    }

    public async Task<UserDto> GetUserByEmail(string email)
    {
      var user = await _context.UsersDb.AsNoTracking()
        .FirstOrDefaultAsync(x => x.Email == email);
      return user?.ToUserDto();

    }

    public async Task<UserDto> GetUserByUserId(int userId)
    {
      var user = await _context.UsersDb.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId);
      return user?.ToUserDto();

    }


    public async Task<UserDto> GetUserByEmailAndPasswordAsync(string email, string passwordHash)
    {
      var user = await _context.UsersDb.AsNoTracking()
        .FirstOrDefaultAsync(x => x.Email == email && x.PasswordHash == passwordHash);
      return user?.ToUserDto();
    }

    public async Task<UserDto> GetUserByPhoneAndPasswordAsync(string phone, string passwordHash)
    {
      var user = await _context.UsersDb.AsNoTracking()
        .FirstOrDefaultAsync(x => x.PhoneNumber == phone && x.PasswordHash == passwordHash);
      return user?.ToUserDto();
    }

    //public async Task<UserDto> GetUserByExternalUserIdAsync(int externalUserId, SocialType socialType)
    //{
    //  UserDb user;
    //  switch (socialType)
    //  {
    //    case SocialType.Facebook:
    //      user = await _context.UsersDb.AsNoTracking().FirstOrDefaultAsync(x => x.FacebookId == externalUserId);
    //      break;
    //    case SocialType.Vk:
    //      user = await _context.UsersDb.AsNoTracking().FirstOrDefaultAsync(x => x.VkId == externalUserId);
    //      break;
    //    case SocialType.Nothing:
    //    case SocialType.Twiter:
    //    case SocialType.GooglePlus:
    //    case SocialType.Telegram:
    //    case SocialType.Badoo:
    //    default:
    //      user = null;
    //      break;
    //  }
    //  return user?.ToUserDto();
    //}

    //public async Task<long> GetExternalUserIdAsync(int userId, SocialType socialType)
    //{
    //  var user = await _context.UsersDb.FindAsync(userId);
    //  if (user == null) return 0;
    //  switch (socialType)
    //  {
    //    case SocialType.Facebook:
    //      return user.FacebookId;
    //    case SocialType.Vk:
    //      return user.VkId;
    //    case SocialType.Twiter:
    //    case SocialType.GooglePlus:
    //    case SocialType.Telegram:
    //    case SocialType.Badoo:
    //    case SocialType.Nothing:
    //    default:
    //      return 0;
    //  }
    //}

    public async Task<bool> IsUserIdExistAsync(int userId)
    {
      return await _context.UsersDb.FindAsync(userId) != null;
    }

    public async Task<bool> IsUserExistByPhoneAsync(string phone)
    {
      return await _context.UsersDb.AsNoTracking().AnyAsync(x => x.PhoneNumber == phone);
    }

    public async Task<bool> IsUserExistByEmailAsync(string email)
    {
      return await _context.UsersDb.AsNoTracking().AnyAsync(x => x.Email == email);
    }

    public async Task<bool> AddUserSocialAsync(SocialDb social)
    {
      _context.SocialsDb.Add(social);
      return await _context.SaveChangesAsync() > 0;
    }

    public async Task<List<UserSocialDto>> GetUserSocialsAsync(int userId)
    {
      return await _context.SocialsDb.Where(x => x.InternalId == userId).Select(x => new UserSocialDto { ExternalId = x.ExternalId, SocialType = x.SocialType }).ToListAsync() ?? new List<UserSocialDto>();
    }

  }
}