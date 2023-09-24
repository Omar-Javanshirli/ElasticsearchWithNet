using Elasticsearch.Web.Repositories;
using Elasticsearch.Web.ViewModel;

namespace Elasticsearch.Web.Services
{
	public class ECommerseService
	{
		private readonly ECommerceRepository repository;

		public ECommerseService(ECommerceRepository repository)
		{
			this.repository = repository;
		}

		public async Task<(List<ECommerseViewModel>, long totalCount, long pageLinkCount)> Searchasync
			(ECommerseSearchViewModel model, int page, int pageSize)
		{
			var (eCommerseList, totalCount) = await repository.SearchAsycn(model, page, pageSize);
			var pageLinkCountCalculate = totalCount % pageSize;
			long pageLinkCount = 0;

			if (pageLinkCountCalculate == 0)
				pageLinkCount = totalCount / pageSize;
			else
				pageLinkCount = (pageLinkCount / pageSize) + 1;

			var ecommerseListViewModel = eCommerseList.Select(x => new ECommerseViewModel()
			{
				Category = string.Join(",", x.Category),
				CustomerFullname = x.CustomerFullname,
				Gender = x.Gender,
				CustomerFirstname = x.CustomerFirstname,
				CustomerLastname = x.CustomerLastname,
				OrderDate = x.OrderDate.ToShortDateString(),
				OrderId = x.OrderId,
				TaxFulTotalPrice = x.TaxFulTotalPrice,
				Id = x.Id,
			}).ToList();

			return (ecommerseListViewModel, totalCount, pageLinkCount);
		}
	}
}
