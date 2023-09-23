using Elasticsearch.Web.Models;
using Elasticsearch.Web.Services;
using Elasticsearch.Web.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;
using System.Text;

namespace Elasticsearch.Web.Controllers
{
	public class BlogController : Controller
	{
		private readonly BlogService _blogService;

		public BlogController(BlogService blogService)
		{
			_blogService = blogService;
		}

		public IActionResult Save()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Save(BlogCreateViewModel model)
		{
			var isSuccess = await _blogService.SaveAsync(model);

			if (!isSuccess)
			{
				TempData["result"] = "save prosesi ugursuzdur";
				return RedirectToAction(nameof(BlogController.Save));
			}

			TempData["result"] = "save prosesi ugurla heyata kecdi";
			return View();
		}

		public async Task<IActionResult> Search()
		{
			return View(await _blogService.SearchAsync(string.Empty));
		}

		[HttpPost]
		public async Task<IActionResult> Search(string searchText)
		{
			ViewBag.SearchText = searchText;
			return View(await _blogService.SearchAsync(searchText));
		}
	}
}