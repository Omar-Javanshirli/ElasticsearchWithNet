using System.Text.Json.Serialization;

namespace Elasticsearch.Web.ViewModel
{
	public class ECommerseViewModel
	{
		public string Id { get; set; } = null!;

		public string CustomerFirstname { get; set; } = null!;

		public string CustomerLastname { get; set; } = null!;

		public string CustomerFullname { get; set; } = null!;

		public string Gender { get; set; }

		public double TaxFulTotalPrice { get; set; }

		public string Category { get; set; } = null!;

		public int OrderId { get; set; }

		public string OrderDate { get; set; }
	}
}
