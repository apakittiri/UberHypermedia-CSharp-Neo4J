using Neo4jClient;
using System;
using System.Configuration;
using System.Web.Http;

namespace UberHypermedia_CSharp_Neo4J
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();


            var url = ConfigurationManager.AppSettings["GraphDBUrl"];
            var client = new GraphClient(new Uri(url), "neo4j", "123456");
            client.Connect();

            GraphClient = client;

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        public static IGraphClient GraphClient { get; private set; }
    }
}
