using Elasticsearch.API.DTOs;
using Elasticsearch.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elasticsearch.API.Controllers
{
    public class Products : BaseController
    {
        private readonly ProductService _productService;

        public Products(ProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult>Save(ProductCreateDto request)
        {
            return CreateActionResult(await _productService.SaveAsync(request));
        }
    }
}
