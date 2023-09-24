using System.ComponentModel.DataAnnotations;

namespace Elasticsearch.Web.ViewModel
{
	public class ECommerseSearchViewModel
	{
		[Display(Name = "Category")]
		public string? Category { get; set; }

		[Display(Name = "Gender")]
		public string? Gender { get; set; }

		[Display(Name = "Order date (start)")]
		[DataType(DataType.Date)]
		public DateTime OrderDateStart { get; set; }

		[Display(Name = "Order date (end)")]
		[DataType(DataType.Date)]
		public DateTime OrderDateEnd { get; set; }

		[Display(Name = "Customer Full name")]
		public string? CustomerFullname { get; set; }
	}
}
