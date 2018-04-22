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
                             .FromSql($"select * from dbo.identification({login}, {int.Parse(password)})").FirstOrDefault()
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

        public static bool TryCheckValidLogin(TwoButtonsContext db, string login, out bool isValid)
        {
            try
            {
                var intIsValid = (db.CheckValidLoginDb
                             .FromSql($"select * from dbo.checkValidLogin({login})").FirstOrDefault()
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