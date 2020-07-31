using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Common.Redis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ManagerController : BaseController
    {
        public ManagerController(RedisHelper client) :base(client)
        {

        }
    }
}
