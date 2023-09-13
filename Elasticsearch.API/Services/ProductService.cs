using Elasticsearch.API.DTOs;
using Elasticsearch.API.Models;
using Elasticsearch.API.Repository;
using System.Net;

namespace Elasticsearch.API.Services
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;

        public ProductService(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ResponseDto<ProductDto>> SaveAsync(ProductCreateDto request)
        {
            var response = await _productRepository.SaveAsync(request.CreateProduct());

            if (response == null)
                return ResponseDto<ProductDto>.Fail(new List<string> { "save zamani bir xeta meydana geldi" },
                    HttpStatusCode.InternalServerError);

            return ResponseDto<ProductDto>.Succsess(response.CreateDto(), HttpStatusCode.Created);
        }
    }
}
