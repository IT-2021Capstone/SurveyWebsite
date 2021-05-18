using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class UserData : IUserData
    {
        private readonly ISQLDataAccess _db;

        public UserData(ISQLDataAccess db)
        {
            _db = db;
        }
        public Task<List<UserModel>> GetUsers()
        {
            //will make a stored procedure later
            string sql = "select * from dbo.UserInfo";
            return _db.LoadData<UserModel, dynamic>(sql, new { });

        }
        public Task InsertUser(UserModel user)
        {
            //will use stored procedure later
            string sql = @"insert into dbo.UserInfo (Name, Email, Address) values (@Name, @Email, @Address);";
            return _db.SaveData(sql, user);
        }
    }
}
