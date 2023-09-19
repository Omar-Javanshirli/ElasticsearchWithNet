using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Elasticsearch.API.Models.ECommerceModel;
using System.Collections.Immutable;

namespace Elasticsearch.API.Repository
{
    public class ECommerseRepository
    {
        private readonly ElasticsearchClient _client;
        private const string indexName = "kibana_sample_data_ecommerce";

        public ECommerseRepository(ElasticsearchClient client)
        {
            _client = client;
        }

        public async Task<ImmutableList<ECommerse>> TermLevelQueryAsync(string customerFirstName)
        {
            //1. way 
            //var result = await _client.SearchAsync<ECommerse>
            //    (x => x.Index(indexName).Query(q => q.Term(t => t.Field("customer_first_name.keyword").Value(customerFirstName))));

            //2. way tip guvenli sekilde
            //var result = await _client.SearchAsync<ECommerse>(x => x.Index(indexName)
            //.Query(q => q.Term(t => t.CustomerFirstname.Suffix("keyword"), customerFirstName)));

            //3. way
            var termQuery = new TermQuery("customer_first_name.keyword")
            { Value = customerFirstName, CaseInsensitive = true };

            var result = await _client.SearchAsync<ECommerse>(x => x.Index(indexName).Query(termQuery));

            foreach (var hit in result.Hits) hit.Source!.Id = hit.Id;

            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerse>> TermsQueryAsync(List<string> customerFirstNameList)
        {
            List<FieldValue> terms = new List<FieldValue>();
            customerFirstNameList.ForEach(customerFirstName =>
            {
                terms.Add(customerFirstName);
            });

            //1. way
            //var termsQuery = new TermsQuery()
            //{
            //    Field = "customer_first_name.keyword",
            //    Terms = new TermsQueryField(terms.AsReadOnly())
            //};
            //var result = await _client.SearchAsync<ECommerse>(s => s.Index(indexName).Query(termsQuery));

            //2.way
            var result = await _client.SearchAsync<ECommerse>(s => s
            .Index(indexName)
            .Size(40)
            .Query(q => q
            .Terms(t => t
            .Field(f => f.CustomerFirstname
            .Suffix("keyword"))
            .Terms(new TermsQueryField(terms.AsReadOnly())))));

            foreach (var hit in result.Hits) hit.Source!.Id = hit.Id;

            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerse>> PrefixQueryAsync(string customerFullname)
        {
            var result = await _client.SearchAsync<ECommerse>(x => x
            .Index(indexName)
            .Query(q => q
            .Prefix(p => p
            .Field(f => f.CustomerFullname
            .Suffix("keyword"))
            .Value(customerFullname))));

            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerse>> RangeQueryAsync(double fromPrice, double toPrice)
        {
            var result = await _client.SearchAsync<ECommerse>(x => x
            .Index(indexName)
            .Query(q => q
            .Range(r => r
            .NumberRange(nr => nr
            .Field(f => f.TaxFulTotalPrice)
            .Gte(fromPrice)
            .Lte(toPrice)))));

            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerse>> MatchAllQueryAsync()
        {
            var result = await _client.SearchAsync<ECommerse>(s => s
            .Index(indexName)
            .Size(100)
            .Query(q => q
            .MatchAll()));

            foreach (var hit in result.Hits) hit.Source!.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerse>> PaginationQueryAsync(int page, int pageSize)
        {
            var pageFrom = (page - 1) * pageSize;

            var result = await _client.SearchAsync<ECommerse>(s => s
            .Index(indexName)
            .Size(pageSize)
            .From(pageFrom)
            .Query(q => q
            .MatchAll()));

            foreach (var hit in result.Hits) hit.Source!.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerse>> WithCardQueryAsync(string customerFullname)
        {
            var result = await _client.SearchAsync<ECommerse>(s => s.Index(indexName)
                 .Query(q => q.Wildcard(w =>
                  w.Field(f => f.CustomerFullname
                  .Suffix("keyword"))
                  .Wildcard(customerFullname))));


            foreach (var hit in result.Hits) hit.Source!.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerse>> FuzzyQueryAsync(string customerName)
        {
            var result = await _client.SearchAsync<ECommerse>(s => s.Index(indexName)
                .Query(q => q
                .Fuzzy(f => f
                .Field(fu => fu.CustomerFirstname
                .Suffix("keyword"))
                .Value(customerName)
                .Fuzziness(new Fuzziness(1))))
                .Sort(sort => sort
                .Field(f => f.TaxFulTotalPrice, new FieldSort() { Order = SortOrder.Desc })));


            foreach (var hit in result.Hits) hit.Source!.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerse>> MatchQuerFullTextAsync(string categoryName)
        {
            var result = await _client.SearchAsync<ECommerse>(s => s
            .Index(indexName)
            .Query(q => q
            .Match(m => m
            .Field(f => f.Category)
            .Query(categoryName)
            .Operator(Operator.And))));

            foreach (var hit in result.Hits) hit.Source!.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerse>> MatchBooleanPrefixFulltextAsync(string customerFullname)
        {
            var result = await _client.SearchAsync<ECommerse>(s => s
            .Index(indexName)
            .Query(q => q
            .MatchBoolPrefix(m => m
            .Field(f => f.Category)
            .Query(customerFullname))));

            foreach (var hit in result.Hits) hit.Source!.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }
    }
}
