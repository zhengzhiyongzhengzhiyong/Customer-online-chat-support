using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Api.TestData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StackExchange.Redis;
using NLog;
using Chat.Common.Redis;

namespace Chat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly Logger DBlogger = LogManager.GetLogger("dblog");

        protected readonly IDatabase _redisUser;

        public BaseController(RedisHelper client)
        {
            _redisUser = client.GetUserDB();
        }

        protected User CurrentUser
        {
            get
            {
                string key = HttpContext.User.FindFirst("id").Value;
                string result = _redisUser.StringGet(key);
                User user = JsonConvert.DeserializeObject<User>(result);
                return user;
            }
        }
    }
}
