using Chat.Api.TestData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Chat.Api.Authentication
{
    public interface IJwtFactory
    {
        Task<string> GenerateEncodedToken(string userName, string refreshToken, ClaimsIdentity identity);
        ClaimsIdentity GenerateClaimsIdentity(User user);
    }
}
