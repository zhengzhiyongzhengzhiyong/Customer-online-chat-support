using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Api.Hubs
{
    public class SignalRUserIdProvider : IUserIdProvider
    {
        public virtual string GetUserId(HubConnectionContext connection)
        {
            string tempstr = connection.User.FindFirst("id").Value;
            return Convert.ToString(tempstr);
        }
    }
}
