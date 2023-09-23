

using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

namespace Elasticsearch.Web.Extensions
{
    public static class ElasticsearchExt
    {
        public static void AddElastic(this IServiceCollection services, IConfiguration Configuration)
        {
            var username = Configuration.GetSection("Elastic")["Username"];
            var password = Configuration.GetSection("Elastic")["Password"];
            var settings = new ElasticsearchClientSettings(new Uri(Configuration.GetSection("Elastic")
                ["Url"]!)).Authentication(new BasicAuthentication(username!,password!));

            var client = new ElasticsearchClient(settings);
            services.AddSingleton(client);
        }
    }
}
