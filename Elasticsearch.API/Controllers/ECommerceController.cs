using Elasticsearch.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Elasticsearch.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ECommerceController : ControllerBase
    {
        private readonly ECommerseRepository _repository;

        public ECommerceController(ECommerseRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> TermQuery(string customerFirstname)
        {
            return Ok(await _repository.TermLevelQueryAsync(customerFirstname));
        }

        [HttpPost]
        public async Task<IActionResult> TermsQuery(List<string> customerFirstnameList)
        {
            return Ok(await _repository.TermsQueryAsync(customerFirstnameList));
        }

        [HttpGet]
        public async Task<IActionResult> PrefixQuery(string customerFullname)
        {
            return Ok(await _repository.PrefixQueryAsync(customerFullname));
        }

        [HttpGet]
        public async Task<IActionResult> RangeQery(double fromPrice, double toPrice)
        {
            return Ok(await _repository.RangeQueryAsync(fromPrice, toPrice));
        }

        [HttpGet]
        public async Task<IActionResult> MatchAllQuery()
        {
            return Ok(await _repository.MatchAllQueryAsync());
        }

        [HttpGet]
        public async Task<IActionResult> PaginationQuery(int page = 1, int pageSize = 3)
        {
            return Ok(await _repository.PaginationQueryAsync(page, pageSize));
        }

        [HttpGet]
        public async Task<IActionResult> WildCardQuery(string customerFullname)
        {
            return Ok(await _repository.WithCardQueryAsync(customerFullname));
        }

        [HttpGet]
        public async Task<IActionResult> fuzzyQuery(string customerName)
        {
            return Ok(await _repository.FuzzyQueryAsync(customerName));
        }


        [HttpGet]
        public async Task<IActionResult> MatchQueryFullText (string categoryName)
        {
            return Ok(await _repository.MatchQuerFullTextAsync(categoryName));
        }


        [HttpGet]
        public async Task<IActionResult> MatchBooleanPrefixFulltext(string categoryFullname)
        {
            return Ok(await _repository.MatchBooleanPrefixFulltextAsync(categoryFullname));
        }
    }
}
