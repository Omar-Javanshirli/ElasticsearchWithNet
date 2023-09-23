using Elasticsearch.Web.Models;
using Elasticsearch.Web.Repositories;
using Elasticsearch.Web.ViewModel;

namespace Elasticsearch.Web.Services
{
    public class BlogService
    {
        private readonly BlogRepository _blogRepository;

        public BlogService(BlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<bool> SaveAsync(BlogCreateViewModel model)
        {
            var newBlog = new Blog()
            {
                Title = model.Title,
                UserId = Guid.NewGuid(),
                Content = model.Content,
                Tags = model.Tags.Split(","),
            };

            var isCreated = await _blogRepository.SaveAsync(newBlog);

            return isCreated != null;
        }

        public async Task<List<BlogSearchViewModel>> SearchAsync(string searchText)
        {
            var blogList= await _blogRepository.SearchAsync(searchText);
			return blogList.Select(b => new BlogSearchViewModel()
			{
				Id = b.Id,
				Title = b.Title,
				Content = b.Content,
				Created = b.Created.ToShortDateString(),
				Tags = String.Join(",", b.Tags),
				UserId = b.UserId.ToString()
			}).ToList();
		}
    }
}
