using Chat.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Api.Extensions
{
    public static class MongoExtensions
    {
        public static IServiceCollection UserMongoLog(this IServiceCollection services, IConfigurationSection configurationSection)
        {
            services.Configure<MongoOptions>(configurationSection);
            services.AddSingleton<MongoContext>();
            return services;
        }
    }
}
