using AS.IdentitySeparation.Infra.CrossCutting.Identity.Filters;
using System.Web.Http;

namespace AS.IdentitySeparation.Application.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Serviços e configuração da API da Web
            //config.Filters.Add(new TokenAuthenticate());

            // Rotas da API da Web
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
