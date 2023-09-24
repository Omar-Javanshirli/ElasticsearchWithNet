using Elasticsearch.Web.Services;
using Elasticsearch.Web.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Elasticsearch.Web.Controllers
{
	public class ECommerseController : Controller
	{
		private readonly ECommerseService _service;

		public ECommerseController(ECommerseService service)
		{
			_service = service;
		}

		public async Task<IActionResult> Search([FromQuery] SearchPageViewModel searchPageView)
		{
			var (eCommerseList,totalCount,pageLinkCount) = await _service.Searchasync
				(searchPageView.SearchViewModel, searchPageView.Page, searchPageView.PageSize);

			searchPageView.List = eCommerseList;
			searchPageView.TotalCount = totalCount;
			searchPageView.PageLinkCount = pageLinkCount;

			return View(searchPageView);
		}
	}
}
