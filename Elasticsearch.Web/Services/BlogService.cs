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

        public async Task<List<Blog>> SearchAsync(string searchText)
        {
            return await _blogRepository.SearchAsync(searchText);
        }
    }
}
