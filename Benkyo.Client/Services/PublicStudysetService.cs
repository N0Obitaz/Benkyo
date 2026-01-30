using Microsoft.Extensions.Caching.Memory;

namespace Benkyo.Client.Services
{
    public class PublicStudysetService
    {
        private readonly HttpClient ht = new HttpClient { BaseAddress = new Uri("localhost://7218") };

        private readonly IMemoryCache _memoryCache;
        public StudysetService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
    }
}
