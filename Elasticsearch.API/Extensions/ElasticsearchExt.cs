using Elasticsearch.Net;
using Nest;

namespace Elasticsearch.API.Extensions
{
    public static class ElasticsearchExt
    {
        public static void AddElastic(this IServiceCollection services,IConfiguration Configuration)
        {

            var pool = new SingleNodeConnectionPool(new Uri(Configuration.GetSection("Elastic")["Url"]!));
            var settings = new ConnectionSettings(pool);
            var client = new ElasticClient(settings);
            services.AddSingleton(client);
        }
    }
}
