using Benkyo.Services;
using Microsoft.Extensions.Caching.Memory;

namespace Benkyo.Controllers
{
    public class PublicFlashcardController
    {
        private readonly FirebaseService _firebaseService;
        private readonly IMemoryCache _memoryCache;

        public PublicFlashcardController(FirebaseService firebaseService, IMemoryCache memoryCache)
        {
            _firebaseService = firebaseService;
            _memoryCache = memoryCache;
        }
    }
}
