using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using Elastic.Clients.Elasticsearch.QueryDsl;
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

			List<Action<QueryDescriptor<Blog>>> listQuery = new();

			Action<QueryDescriptor<Blog>> matchAll = (q) => q.MatchAll();

			Action<QueryDescriptor<Blog>> matchContent = (q) =>
			{
				q.Match(m => m
				.Field(f => f.Content)
				.Query(searchText));
			};

			Action<QueryDescriptor<Blog>> titleMatchBollPrefix = (q) =>
			{
				q.MatchBoolPrefix(mp => mp
				.Field(f => f.Content)
				.Query(searchText));
			};

			Action<QueryDescriptor<Blog>> tagTerm = (q) =>
			{
				q.Term(t => t
				.Field(f => f.Tags)
				.Value(searchText));
			};

			if (string.IsNullOrEmpty(searchText))
				listQuery.Add(matchAll);

            else
            {
				listQuery.Add(matchContent);
				listQuery.Add(titleMatchBollPrefix);
				listQuery.Add(tagTerm);
            }

            var result = await _elasticSearchClient.SearchAsync<Blog>(s => s
		   .Index(indexName)
		   .Query(q => q
		   .Bool(b => b
		   .Should(listQuery.ToArray()))));

			foreach (var hit in result.Hits) hit.Source!.Id = hit.Id;
			return result.Documents.ToList();
		}
	}
}
