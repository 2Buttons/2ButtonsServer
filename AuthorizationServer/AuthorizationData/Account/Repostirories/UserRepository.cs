using System;
using System.Threading.Tasks;
using AuthorizationData.Account.DTO;
using AuthorizationData.Account.Entities;
using CommonLibraries;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationData.Account.Repostirories
{
  public class UserRepository : IDisposable
  {
    private readonly TwoButtonsAccountContext _contextAccount;

    public UserRepository(TwoButtonsAccountContext context)
    {
      _contextAccount = context;
    }

    public void Dispose()
    {
      _contextAccount.Dispose();
    }

    public async Task<bool> AddEmail(string email)
    {
      _contextAccount.EmailsDb.Add(new EmailDb {Email = email});
      return await _contextAccount.SaveChangesAsync() > 0;
    }

    public async Task<bool> ConfirmEmail(int userId)
    {
      var user = await _contextAccount.UsersDb.FindAsync(userId);
      if (user == null) return false;
      user.IsEmailConfirmed = true;
      _contextAccount.Entry(user).State = EntityState.Modified;
      return await _contextAccount.SaveChangesAsync() > 0;
    }

    public async Task<bool> AddUserAsync(UserDb user)
    {
      await _contextAccount.UsersDb.AddAsync(user);
      return await _contextAccount.SaveChangesAsync() > 0;
    }

    public async Task<bool> ResetPasswordAsync(string email, string passwordHash)
    {
      var user = await _contextAccount.UsersDb.FirstOrDefaultAsync(x=>x.Email == email);
      if (user == null || !user.IsEmailConfirmed) return false;

      user.PasswordHash = passwordHash;
      _contextAccount.Entry(user).State = EntityState.Modified;
      return await _contextAccount.SaveChangesAsync() > 0;
    }

    public async Task<bool> AddSocialAsync(SocialDb social)
    {
      await _contextAccount.SocialsDb.AddAsync(social);
      return await _contextAccount.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateSocialAsync(SocialDb social)
    {
      var socialDb = await _contextAccount.SocialsDb.FirstOrDefaultAsync(x =>x.ExternalId == social.ExternalId && x.SocialType == social.SocialType);
      if (socialDb == null) return false;

      socialDb.Email = socialDb.Email == social.Email ? socialDb.Email : social.Email;
      socialDb.PhoneNumber = socialDb.PhoneNumber == social.PhoneNumber ? socialDb.PhoneNumber : social.PhoneNumber;
      socialDb.ExternalToken = socialDb.ExternalToken == social.ExternalToken ? socialDb.ExternalToken : social.ExternalToken;
      socialDb.ExpiresIn = socialDb.ExpiresIn == social.ExpiresIn ? socialDb.ExpiresIn : social.ExpiresIn;
      _contextAccount.Entry(social).State = EntityState.Modified;
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
      user.IsEmailConfirmed = emailConfirmed;
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

    public async Task<UserDto> GetUserByInternalEmail(string email)
    {
      var user = await _contextAccount.UsersDb
        .FirstOrDefaultAsync(x => x.Email == email);
      return user?.ToUserDto();
    }

    public async Task<UserDto> GetUserByUserId(int userId)
    {
      var user = await _contextAccount.UsersDb.FirstOrDefaultAsync(x => x.UserId == userId);
      return user?.ToUserDto();
    }


    public async Task<UserDto> GetUserByInternalEmailAndPasswordAsync(string email, string passwordHash)
    {
      var user = await _contextAccount.UsersDb
        .FirstOrDefaultAsync(x => x.Email == email && x.PasswordHash == passwordHash);
      return user?.ToUserDto();
    }

    public async Task<UserDto> GetUserByInernalPhoneAndPasswordAsync(string phone, string passwordHash)
    {
      var user = await _contextAccount.UsersDb
        .FirstOrDefaultAsync(x => x.PhoneNumber == phone && x.PasswordHash == passwordHash);
      return user?.ToUserDto();
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

    public async Task<bool> RemoveUserAsync(int userId)
    {
      var user = await _contextAccount.UsersDb.FindAsync(userId);
      if (user == null) return false;
      _contextAccount.UsersDb.Remove(user);
      return await _contextAccount.SaveChangesAsync() > 0;
    }
  }
}