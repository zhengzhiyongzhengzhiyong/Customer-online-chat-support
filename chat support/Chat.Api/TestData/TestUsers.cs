using Chat.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Api.TestData
{
    public static class TestUsers
    {
        public static List<User> Users = new List<User>
        {
            new User{ Id ="5a418c17-0193-4f1f-bf64-fe143385d4bd", UserName = "Support", Password = "qwe123", Roles = new List<string>{ "administrator"}},
            new User{ Id ="dbd6f4e8-9867-43c9-ae36-5839c990d6fb", UserName = "me", Password = "Young123", Roles = new List<string>{ "api_access" }},
            new User{ Id = "8f751f74-fd52-41cc-8028-196c47677007", UserName = "user2", Password = "Roy123", Roles = new List<string>{ "administrator" }},
        };
    }

    public class User
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<string> Roles { get; set; }
    }
}
