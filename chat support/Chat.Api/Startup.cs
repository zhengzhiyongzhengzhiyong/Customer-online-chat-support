using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Chat.Api.Authentication;
using Chat.Api.Controllers;
using Chat.Api.Extensions;
using Chat.Api.Hubs;
using Chat.Api.Middleware;
using Chat.Api.TestData;
using Chat.Common.Redis;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using IUserIdProvider = Microsoft.AspNetCore.SignalR.IUserIdProvider;

namespace Chat.Api
{
    public class Startup
    {

        private const string SecretKey = "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH"; 

        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(SecretKey));

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSignalR();

            services.AddSingleton<IJwtFactory, JwtFactory>();

            services.AddTransient<IUserService, UserService>();

            var section = Configuration.GetSection("Redis:Default");

            string _connectionString = section.GetSection("Connection").Value;

            string _instanceName = section.GetSection("InstanceName").Value;

            services.AddSingleton(new RedisHelper(_connectionString, _instanceName));

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = section.GetSection("Connection").Value;
                options.InstanceName = "CacheInstance";
            });

            services.AddCors(c =>
            {
                c.AddPolicy("AllRequests", policy =>
                {
                    policy
                    .WithOrigins("http://localhost:8080")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
            });

            services.AddSwaggerGen(options => {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "API Docs",
                    Version = "v1",
                });

                var security = new Dictionary<string, IEnumerable<string>> { { "Bearer", new string[] { } }, };
                options.AddSecurityRequirement(security);
                options.AddSecurityDefinition("Bearer", new Swashbuckle.AspNetCore.Swagger.ApiKeyScheme
                {
                    Description = "Format: Bearer {auth_token}",
                    Name = "Authorization",
                    In = "header"
                });
            });

            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],
                    ValidateAudience = false,
                    ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],
                    ValidateIssuerSigningKey = false,
                    IssuerSigningKey = _signingKey,
                    RequireExpirationTime = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                configureOptions.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        if (!string.IsNullOrEmpty(accessToken) && (context.HttpContext.Request.Path.StartsWithSegments("/chatHub")))
                        {
                            context.Token = accessToken;
                        }
                        else if (!string.IsNullOrEmpty(accessToken) && (context.HttpContext.Request.Path.StartsWithSegments("/chatroom")))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.UserMongoLog(Configuration.GetSection("Mongodb"));

            services.AddSingleton(typeof(ConnectionList));

            services.AddSingleton<IUserIdProvider, SignalRUserIdProvider>();

            services.AddSignalR(options=> { options.EnableDetailedErrors = true; });

            var builder = new ContainerBuilder();

            var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
            try
            {
                var IservicesDllFile = Path.Combine(basePath, "Chat.IServices.dll");
                var servicesDllFile = Path.Combine(basePath, "Chat.Services.dll");
                var assemblysServices = Assembly.LoadFile(servicesDllFile);
                var assemblysIServices = Assembly.LoadFile(IservicesDllFile);
                builder.RegisterAssemblyTypes(assemblysServices, assemblysIServices)
                                .AsImplementedInterfaces();
            }
            catch (Exception)
            {
                throw new Exception("※※★※※ 请先 build Chat.Services.dll，然后,检查bin文件夹是否存在Chat.Services.dll ※※★※※");
            }

            builder.Populate(services);

            var ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseSwagger().UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });

            app.UseGlobalErrorHandlingMiddleware();

            app.UseAuthentication();

            app.UseCors("AllRequests");

            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chathub");
                routes.MapHub<ChatRoom>("/chatroom");
            });

            app.UseMvc();
        }
    }
}
