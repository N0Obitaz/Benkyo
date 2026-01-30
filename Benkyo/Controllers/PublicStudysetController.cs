using Benkyo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Benkyo.Controllers
{
    [ApiController]
    [Route("api[controller]")]
    public class PublicStudysetController : ControllerBase
    {
        private readonly FirebaseService _firebaseService;
        private readonly IMemoryCache _memoryCache;

        public PublicStudysetController(FirebaseService firebaseService, IMemoryCache memoryCache)
        {
            _firebaseService = firebaseService;
            _memoryCache = memoryCache;
        }

        //Fetch 10 rows of studysets only
        [HttpGet]
        public async Task<IActionResult> FetchPublicStudysets()
        {
            var studysetRef = _firebaseService._db.Collection("studysets");

            var snapshot = await studysetRef.GetSnapshotAsync();
            return Ok();
        }

    }
}
