using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Elasticsearch.Web.Models;
using Elasticsearch.Web.ViewModel;

namespace Elasticsearch.Web.Repositories
{
	public class ECommerceRepository
	{
		private readonly ElasticsearchClient _elasticSearchClient;
		private const string IndexName = "kibana_sample_data_ecommerce";

		public ECommerceRepository(ElasticsearchClient elasticSearchClient)
		{
			_elasticSearchClient = elasticSearchClient;
		}

		public async Task<(List<ECommerse> list , long count)> SearchAsycn(ECommerseSearchViewModel model, int page, int pageSize)
		{
			List<Action<QueryDescriptor<ECommerse>>> listQuery = new();

			if(model is null)
			{
				listQuery.Add(g => g.MatchAll());
                return await CalculateResultSet(page, pageSize, listQuery);
            }

			if (!string.IsNullOrEmpty(model.Category))
			{
				listQuery.Add((q) =>
				{
					q.Match(m => m
					.Field(f => f.Category)
					.Query(model.Category));
				});
			}

			if (!string.IsNullOrEmpty(model.CustomerFullname))
			{
				listQuery.Add((q) =>
				{
					q.Match(m => m
						.Field(f => f.CustomerFullname)
						.Query(model.CustomerFullname));
				});
			}

			if (model.OrderDateStart != null)
			{
				listQuery.Add((q) =>
				{
					q.Range(r => r
						.DateRange(dr => dr
							.Field(f => f.OrderDate)
							.Gte(model.OrderDateStart)));
				});
			}

			if (model.OrderDateEnd != null)
			{
				listQuery.Add((q) =>
				{
					q.Range(r => r
						.DateRange(dr => dr
							.Field(f => f.OrderDate)
							.Lte(model.OrderDateEnd)));
				});
			}

			if (!string.IsNullOrEmpty(model.Gender))
			{
				listQuery.Add((q) =>
				{
					q.Term(t => t
						.Field(f => f.Gender)
						.Value(model.Gender));
				});
			}

			if(!listQuery.Any())
                listQuery.Add(g => g.MatchAll());


			return await CalculateResultSet(page, pageSize, listQuery);
		}

		private async Task<(List<ECommerse> list,long count)>CalculateResultSet(int page,int pageSize,
			List<Action<QueryDescriptor<ECommerse>>>listQuery)
		{

            var pageForm = (page - 1) * pageSize;
            var result = await _elasticSearchClient.SearchAsync<ECommerse>(s => s.Index(IndexName)
                .Size(pageSize)
                .From(pageForm)
                    .Query(q => q
                        .Bool(b => b
                            .Must(listQuery.ToArray()))));

            foreach (var hit in result.Hits) hit.Source!.Id = hit.Id;
			return (list: result.Documents.ToList(), result.Total);
        }
	}
}
