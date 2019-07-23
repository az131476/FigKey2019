using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using WebAPISwaggerUI;
using WebAPISwaggerUI.Controllers;
using WebAPISwaggerUI.Areas;
using System.Configuration;

[assembly: OwinStartup(typeof(WebAPISwaggerUI.Startup))]

namespace WebAPISwaggerUI
{
    public class Startup
    {
        public IConfigurationSystem configuration;
        public void Configuration(IAppBuilder app)
        {
            // 有关如何配置应用程序的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkID=316888
            
        }

        public Startup(IConfigurationSystem cfg)
        {
            configuration = cfg;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info() { Title = "Swagger Test UI", Version = "v1" });
                options.CustomSchemaIds(type => type.FullName); // 解决相同类名会报错的问题
                options.IncludeXmlComments(Path.Combine(Directory.GetCurrentDirectory(), "SwaggerUIDemo.xml")); // 标注要使用的 XML 文档
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseSwagger();
            // 在这里面可以注入
            app.UseSwaggerUI(options =>
            {
                options.InjectOnCompleteJavaScript("/swagger/ui/zh_CN.js"); // 加载中文包
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "HKERP API V1");
            });
            app.UseMvc();
        }

    }
}
