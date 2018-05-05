using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TwoButtonsDatabase.Entities;

namespace TwoButtonsDatabase.WrapperFunctions
{
    public static class LoginWrapper
    {
        public static bool TryGetIdentification(TwoButtonsContext db, string login, string password, out int userId)
        {
            try
            {
                userId = db.IdentificationDb
                             .FromSql($"select * from dbo.identification({login}, {password})").FirstOrDefault()
                             ?.UserId ?? -1;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            userId = -1;
            return false;
        }

        public static bool TryCheckValidUser(TwoButtonsContext db, string phone, string login, out int returnsCode)
        {
            try
            {
                returnsCode = db.CheckValidUserDb
                             .FromSql($"select * from dbo.checkValidUser({phone}, {login})").FirstOrDefault()
                             ?.ReturnCode ?? -1;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            returnsCode = -1;
            return false;
        }

        public static bool TryIsUserIdValid(TwoButtonsContext db, int userId, out bool isValid)
        {
            try
            {
                var intIsValid = (db.IsUserIdValidDb
                             .FromSql($"select * from dbo.isUserIdValid({userId})").FirstOrDefault()
                             ?.IsValid  ?? 0);
                isValid = intIsValid == 1;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            isValid = false;
            return false;
        }
    }
}