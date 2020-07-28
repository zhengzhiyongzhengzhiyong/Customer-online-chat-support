using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Chat.Api.Authentication;
using Chat.Api.Models;
using Chat.Api.TestData;
using Chat.Common;
using Chat.Common.Redis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Chat.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly IUserService _userService;
        private IDistributedCache _cache;
        private readonly IDatabase _redis;

        public AuthController(RedisHelper client,IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions, IUserService userService, IDistributedCache cache)
        {
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
            _userService = userService;
            _cache = cache;
            _redis = client.GetUserDB();
        }

        /// <summary>
        /// 测试缓存
        /// </summary>
        /// <returns></returns>
        [HttpGet("Index")]
        public async Task<ActionResult<string>> SetTime()
        {
            var CurrentTime = DateTime.Now.ToString();
            await this._cache.SetStringAsync("CurrentTime", CurrentTime);
            return CurrentTime;
        }

        /// <summary>
        /// 测试缓存
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTime")]
        public async Task<ActionResult<string>> GetTime()
        {
            var CurrentTime = await this._cache.GetStringAsync("CurrentTime");
            return CurrentTime;
        }

        [HttpPost]
        public IActionResult GetSupport()
        {
            List<chater> chaters = new List<chater>();
            chaters.Add(new chater { id = "5a418c17-0193-4f1f-bf64-fe143385d4bd", name = "Support", imageUrl = "https://avatars3.githubusercontent.com/u/1915989?s=230&v=4" });
            chaters.Add(new chater { id = "dbd6f4e8-9867-43c9-ae36-5839c990d6fb", name = "me", imageUrl = "https://avatars3.githubusercontent.com/u/1915989?s=230&v=4" });
            var result = JsonConvert.SerializeObject(new { data = chaters }, new JsonSerializerSettings { Formatting = Formatting.Indented });
            return new OkObjectResult(result);
        }

        /// <summary>
        /// Log in
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginRequest request)
        {
            var user = _userService.GetUserByName(request.UserName);
            if (user == null)
            {
                ModelState.AddModelError("login_failure", "Invalid username.");
                return BadRequest(ModelState);
            }
            if (!request.Password.Equals(user.Password))
            {
                ModelState.AddModelError("login_failure", "Invalid password.");
                return BadRequest(ModelState);
            }

            var claimsIdentity = _jwtFactory.GenerateClaimsIdentity(user);

            var token = await _jwtFactory.GenerateEncodedToken(user.UserName, claimsIdentity);

            await _redis.StringSetAsync(user.Id.ToString(), JsonConvert.SerializeObject(user), TimeSpan.FromHours(2));

            var response = new
            {
                auth_token = token,
                token_type = "Bearer"
            };
            var result = JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented });
            return new OkObjectResult(result);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult getUserInfo()
        {
            return new OkObjectResult(new data{ name="Jackson", avatar = "https://emojipedia-us.s3.dualstack.us-west-1.amazonaws.com/thumbs/120/apple/237/frame-with-picture_1f5bc.png" });
        }


        /// <summary>
        /// Get User Info
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IActionResult GetUserInfo()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            return Ok(claimsIdentity.Claims.ToList().Select(r => new { r.Type, r.Value }));
        }
    }
}
