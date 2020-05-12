using Microsoft.Extensions.Caching.Memory;
using Portfolio.Components.Classes;
using Portfolio.Shared.ComponentModels;
using Portfolio.Shared.Interfaces;
using Portfolio.Shared.PageModels;
using System.Threading.Tasks;

namespace Portfolio.Components.Services
{
    public abstract class ContentService
    {
        protected readonly PortfolioApiClient _apiClient;
        protected readonly IMemoryCache _cache;

        public ContentService(PortfolioApiClient apiClient, IMemoryCache cache)
        {
            _cache = cache;
            _apiClient = apiClient;
        }

        public abstract Task<HomePageModel> GetHomePageAsync();
        public abstract Task<BlogModel> GetBlogModelAsync(int takePosts);
        public abstract Task<BlogPageModel> GetBlogPageByIdAsync(int id);
        public abstract Task<IComponentModel> GetFlexibleModelByIdAsync(int id);
        public abstract Task<HeroModel> GetHeroModelByIdAsync(int id);
    }
}
