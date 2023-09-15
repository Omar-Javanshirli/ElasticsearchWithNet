using Elasticsearch.API.DTOs;
using Elasticsearch.API.Models;
using Elasticsearch.API.Repository;
using Nest;
using System.Collections.Immutable;
using System.Net;

namespace Elasticsearch.API.Services
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(ProductRepository productRepository, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<ResponseDto<ProductDto>> SaveAsync(ProductCreateDto request)
        {
            var response = await _productRepository.SaveAsync(request.CreateProduct());

            if (response == null)
                return ResponseDto<ProductDto>.Fail(new List<string> { "save zamani bir xeta meydana geldi" },
                    HttpStatusCode.InternalServerError);

            return ResponseDto<ProductDto>.Succsess(response.CreateDto(), HttpStatusCode.Created);
        }

        public async Task<ResponseDto<List<ProductDto>>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();
            var productListDto = new List<ProductDto>();


            foreach (var x in products)
            {
                if (x.Feature is null)
                {
                    productListDto.Add(new ProductDto(x.Id, x.Name, x.Price, x.Stock, null));
                    continue;
                }

                productListDto.Add(new ProductDto(x.Id, x.Name, x.Price, x.Stock,
                    new ProductFeatureDto(x.Feature.Width, x.Feature.Height, x.Feature.Color)));
            }

            return ResponseDto<List<ProductDto>>.Succsess(productListDto, HttpStatusCode.OK);
        }

        public async Task<ResponseDto<ProductDto>> GetByIdAsync(string id)
        {
            var hasProduct = await _productRepository.GetByIdAsync(id);

            if (hasProduct == null)
                return ResponseDto<ProductDto>.Fail("product tapilmadi", HttpStatusCode.NotFound);

            return ResponseDto<ProductDto>.Succsess(hasProduct.CreateDto(), HttpStatusCode.OK);
        }

        public async Task<ResponseDto<bool>> UpdateAsync(ProductUpdateDto updateProduct)
        {
            var isSuccess = await _productRepository.UpdateAsync(updateProduct);

            if (isSuccess == false)
                return ResponseDto<bool>.Fail("update zamani xeta bas verdi", HttpStatusCode.InternalServerError);

            return ResponseDto<bool>.Succsess(true, HttpStatusCode.NoContent);
        }

        public async Task<ResponseDto<bool>>DeleteAsync(string id)
        {
            var deleteResponse = await _productRepository.DeleteAsync(id);

            if (!deleteResponse.IsValid && deleteResponse.Result==Result.NotFound)
            {
                return ResponseDto<bool>.Fail("silmeye calisdiginiz product tapilmadi", 
                    HttpStatusCode.NotFound);
            }

            if(!deleteResponse.IsValid)
            {
                _logger.LogError(deleteResponse.OriginalException, deleteResponse.ServerError.ToString());
                return ResponseDto<bool>.Fail("silme zamani xeta bas verdi", HttpStatusCode.InternalServerError);
            }

            return ResponseDto<bool>.Succsess(true, HttpStatusCode.NoContent);
        }
    }
}
