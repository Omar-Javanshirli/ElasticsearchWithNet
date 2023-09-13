using Elasticsearch.API.Models;
using Nest;

namespace Elasticsearch.API.Repository
{
    public class ProductRepository
    {
        private readonly ElasticClient _client;

        public ProductRepository(ElasticClient client)
        {
            _client = client;
        }

        public async Task<Product?> SaveAsync(Product newProduct)
        {
            newProduct.Created = DateTime.Now;

            var response = await _client.IndexAsync(newProduct, x => x.Index("products"));
            
            //fast fail
            if (response is { IsValid: false }) return null;

            newProduct.Id = response.Id;
            return newProduct;
        }
    }
}
