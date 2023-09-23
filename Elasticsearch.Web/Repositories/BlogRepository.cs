using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using Elasticsearch.Web.Models;

namespace Elasticsearch.Web.Repositories
{
    public class BlogRepository
    {
        private readonly ElasticsearchClient _elasticSearchClient;
        private const string indexName = "blog";

        public BlogRepository(ElasticsearchClient elasticSearchClient)
        {
            _elasticSearchClient = elasticSearchClient;
        }

        public async Task<Blog?> SaveAsync(Blog newBLog)
        {
            newBLog.Created = DateTime.Now;

            var response = await _elasticSearchClient.IndexAsync
                (newBLog, x => x.Index(indexName).ToString());

            //fast fail
            if (!response.IsValidResponse) return null;

            newBLog.Id = response.Id;
            return newBLog;
        }

        public async Task<List<Blog>> SearchAsync(string searchText)
        {
            // Should  (term1 and term2 and term3); "." noqte qoydugumuz zaman Shouldun icerisende olan
            // sorgulari "and" kimi qebul edir. Eger "or" olsun isdeyirikse "," qoymaq lazimdir.

            var result = await _elasticSearchClient.SearchAsync<Blog>(s => s
           .Index(indexName)
           .Query(q => q
           .Bool(b => b
           .Should(s => s
                .Match(m => m
                    .Field(f => f.Content)
                    .Query(searchText)),
                s => s.MatchBoolPrefix(p => p
                    .Field(f => f.Title
                    .Suffix("keyword"))
                    .Query(searchText))))));

            foreach (var hit in result.Hits) hit.Source!.Id = hit.Id;
            return result.Documents.ToList();
        }
    }
}
