using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Api.TestData
{
    public interface IUserService
    {
        User GetUserByName(string name);
        List<string> GetFunctionsByUserId(int id);
    }

    public class UserService : IUserService
    {
        public List<string> GetFunctionsByUserId(int id)
        {
            return null;
        }

        public User GetUserByName(string name)
        {
            var user = TestUsers.Users.SingleOrDefault(r => r.UserName.Equals(name));
            return user;
        }
    }
}
