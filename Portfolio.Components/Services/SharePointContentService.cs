using Portfolio.Shared.ComponentModels;
using Portfolio.Shared.PageModels;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Portfolio.Shared.Interfaces;
using Portfolio.Components.Classes;

namespace Portfolio.Components.Services
{
    public class SharePointContentService : ContentService
    {
        public SharePointContentService(PortfolioApiClient apiClient, IMemoryCache cache) : base(apiClient, cache)
        { }

        private readonly TimeSpan _cacheTimeSpan = TimeSpan.FromMinutes(20);

        public override async Task<HomePageModel> GetHomePageAsync()
        {
            return await _cache.GetOrCreateAsync("Homepage", async entry =>
            {
                entry.SetAbsoluteExpiration(_cacheTimeSpan);
                return await _apiClient.Client.GetJsonAsync<HomePageModel>("api/homePage");
            });
        }

        public async override Task<BlogModel> GetBlogModelAsync(int take)
        {
            return await _cache.GetOrCreateAsync($"BlogComponent-{take}", async entry =>
            {
                entry.SetAbsoluteExpiration(_cacheTimeSpan);
                return await _apiClient.Client.GetJsonAsync<BlogModel>($"api/blogComponent/{take}");
            });
        }

        public async override Task<BlogPageModel> GetBlogPageByIdAsync(int id)
        {
            return await _cache.GetOrCreateAsync($"BlogPage-{id}", async entry =>
            {
                entry.SetAbsoluteExpiration(_cacheTimeSpan);
                return await _apiClient.Client.GetJsonAsync<BlogPageModel>($"api/blogPages/{id}");
            });
        }

        public async override Task<IComponentModel> GetFlexibleModelByIdAsync(int id)
        {
            return await _cache.GetOrCreateAsync($"FlexibleComponent-{id}", async entry =>
            {
                entry.SetAbsoluteExpiration(_cacheTimeSpan);
                return await _apiClient.Client.GetJsonAsync<IComponentModel>($"api/flexComponent/{id}");
            });
        }

        public override async Task<HeroModel> GetHeroModelByIdAsync(int id)
        {
            return await _cache.GetOrCreateAsync($"HeroComponent-{id}", async entry =>
            {
                entry.SetAbsoluteExpiration(_cacheTimeSpan);
                return await _apiClient.Client.GetJsonAsync<HeroModel>($"api/heroComponent/{id}");
            });
        }
    }
}
