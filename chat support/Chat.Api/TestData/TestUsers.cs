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
            new User{ Id = 1, UserName = "Paul", Password = "Paul123", Roles = new List<string>{ "administrator", "api_access" }, Urls = new List<string>{ "/api/values/getadminvalue", "/api/values/getguestvalue" }},
            new User{ Id = 2, UserName = "Young", Password = "Young123", Roles = new List<string>{ "api_access" }, Urls = new List<string>{ "/api/values/getguestvalue" }},
            new User{ Id = 3, UserName = "Roy", Password = "Roy123", Roles = new List<string>{ "administrator" }, Urls = new List<string>{ "/api/values/getadminvalue" }},
        };
    }

    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<string> Roles { get; set; }
        public List<string> Urls { get; set; }
    }
}
