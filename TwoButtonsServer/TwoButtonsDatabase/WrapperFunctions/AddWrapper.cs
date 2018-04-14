using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace TwoButtonsDatabase.WrapperFunctions
{
    public static  class AddWrapper
    {

        public static bool TryAddUser(TwoButtonsContext db, string login, string password, int age, int sex, string phone, string description, string avatarLink, out int userId)
        {
            
                var userIdDb = new SqlParameter
                {
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output,
                };

                var regDate = DateTime.Now;

                try
                {
                    db.Database.ExecuteSqlCommand($"addUser {login}, {password}, {age}, {sex}, {phone}, {description}, {avatarLink}, {regDate}, {userIdDb} OUT");
                    userId = (int)userIdDb.Value;
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                userId = -1;
                return false;
            

            //using (TwoButtonsContext db = new TwoButtonsContext())
            //{

            //    var loginDb = new SqlParameter("@login", login);
            //    var passwordDb = new SqlParameter("@password", password);
            //    var ageDb = new SqlParameter("@age", age);
            //    var sexDb = new SqlParameter("@sex", sex);
            //    var phoneDb = new SqlParameter("@phone", phone);
            //    var descriptionDb = new SqlParameter("@description", description);
            //    var avatarLinkDb = new SqlParameter("@avatarLink", avatarLink);
            //    var regDate = DateTime.Now;
            //    var regDateDb = new SqlParameter("@regDate", regDate);

            //    var userIdDb = new SqlParameter
            //    {
            //        ParameterName = "@userID",
            //        SqlDbType = SqlDbType.Int,
            //        Direction = ParameterDirection.Output,
            //    };

            //    try
            //    {
            //        db.Database.ExecuteSqlCommand("addUser @login, @password, @age, @sex, @phone, @description, @avatarLink, @regDate, @userID OUT", loginDb, passwordDb, ageDb, sexDb, phoneDb, descriptionDb, avatarLinkDb, regDateDb, userIdDb);
            //        userId = (int)userIdDb.Value;
            //        return true;
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e);
            //    }

            //    userId = -1;
            //    return false;
            //}
        }

        public static bool TryAddQuestion(TwoButtonsContext db, int userId, string condition, int anonymity, int audience, int questionType, int standartPictureId, string firstOption, string secondOption)
        {
               var userIdDb = new SqlParameter("@userID", userId);
                var conditionDb = new SqlParameter("@condition", condition);
                var anonymityDb = new SqlParameter("@anonymity", anonymity);
                var questionTypeDb = new SqlParameter("@questionType", questionType);
                var standartPictureIdDb = new SqlParameter("@standartPictureID", standartPictureId);
                var firstOptionDb = new SqlParameter("@firstOption", firstOption);
                var secondOptionDb = new SqlParameter("@secondOption", secondOption);
                var askedTime = DateTime.Now;
                var askedTimeDb = new SqlParameter("@asked", askedTime);

                try
                {
                    db.Database.ExecuteSqlCommand(
                        "addQuestion @userID, @condition, @anonymity, @questionType, @standartPictureID, @firstOption, @secondOption, @asked",
                        userIdDb,
                        conditionDb,
                        anonymityDb,
                        questionTypeDb,
                        standartPictureIdDb,
                        firstOptionDb,
                        secondOptionDb,
                        askedTimeDb);

                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                return false;
            }
        

    }
}


//System.Data.SqlClient.SqlParameter param = new System.Data.SqlClient.SqlParameter("@price", 50000);
//var phones = Db.Comment.FromSql("SELECT * FROM GetPhonesByPrice (@price)", param).ToList();
//foreach (var p in phones)

