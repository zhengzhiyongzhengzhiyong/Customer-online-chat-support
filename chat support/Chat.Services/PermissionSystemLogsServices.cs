using Chat.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Services
{
    public class PermissionSystemLogsServices : BaseService<PermissionSystemLogs>
    {
        public PermissionSystemLogs(IMongoCollection<PermissionSystemLogs> collection)
        {
            DbSet = collection;
            DbContext = collection.Database;
        }
    }
}
