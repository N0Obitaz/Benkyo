using Benkyo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Shared.Models;

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


            List<Studyset> studysetsPer10 = new();
            var studysetRef = _firebaseService._db.Collection("studysets");

            var query = studysetRef.WhereEqualTo("visibility", "Private");

            var snapshot = await studysetRef.GetSnapshotAsync();

            foreach(var s in snapshot)
            {
                studysetsPer10.Add(new Studyset
                {
                    Id = s.Id,
                    StudySetName = s.GetValue<string>("studyset_name"),
                    StudySetColor = s.GetValue<string>("studyset_color"),
                    UserId = s.GetValue<string>("user_id"),
                    FlashcardCount = s.GetValue<int>("total_flashcards"),

                });
            }
            return Ok(studysetsPer10);
        }

    }
}
