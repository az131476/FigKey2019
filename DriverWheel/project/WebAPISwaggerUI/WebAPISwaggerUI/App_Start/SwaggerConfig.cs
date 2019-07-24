using System.Web.Http;
using WebActivatorEx;
using WebAPISwaggerUI;
using Swashbuckle.Application;
using System.Linq;


[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace WebAPISwaggerUI
{
    public class SwaggerConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableSwagger(c =>
            {
                c.SingleApiVersion("v1", "");
                c.IncludeXmlComments(GetXmlCommentsPath());
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            })
                .EnableSwaggerUi();
        }
        private static string GetXmlCommentsPath()
        {
            return System.String.Format(@"{0}\Swagger.XML", System.AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
