using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.JwtAuthorize;

[assembly: OwinStartup(typeof(WebApiJwtAuthorizeTest.Startup))]

namespace WebApiJwtAuthorizeTest
{
    public class Startup
    {
        //public void Configuration(IAppBuilder app)
        //{
        //    // 有关如何配置应用程序的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkID=316888
        //}
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApiJwtAuthorize((context) =>
            {
                return ValidatePermission(context);
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
        /// <summary>
        /// Cusomer Validate Method
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        bool ValidatePermission(HttpContext httpContext)
        {
            var permissions = new List<Permission>() {
                new Permission { Name="admin", Predicate="Get", Url="/api/values" },
                new Permission { Name="admin", Predicate="Post", Url="/api/values" }
            };
            var questUrl = httpContext.Request.Path.Value.ToLower();

            if (permissions != null && permissions.Where(w => w.Url.Contains("}") ? questUrl.Contains(w.Url.Split('{')[0]) : w.Url.ToLower() == questUrl && w.Predicate.ToLower() == httpContext.Request.Method.ToLower()).Count() > 0)
            {
                var roles = httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Role).Value;
                var roleArr = roles.Split(',');
                var perCount = permissions.Where(w => roleArr.Contains(w.Name)).Count();
                if (perCount == 0)
                {
                    httpContext.Response.Headers.Add("error", "no permission");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
