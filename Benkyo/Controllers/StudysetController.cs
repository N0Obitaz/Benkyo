using Benkyo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Shared.Models;

namespace Benkyo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudysetController : ControllerBase
    {
        private readonly FirebaseService _firebaseService;

        private readonly IMemoryCache _memoryCache;



        public StudysetController(FirebaseService firebaseService, IMemoryCache memoryCache)
        {
            _firebaseService = firebaseService;
            _memoryCache = memoryCache;
        }
        

        [HttpPost("create")]
        public async Task<IActionResult> CreateStudySet([FromBody] Studyset request)
        {
            
            
            try
            {

                var existing = await _firebaseService._db.Collection("studysets")
                .GetSnapshotAsync();
                if (existing == null)
                    return BadRequest(new { Message = "Failed to create study set" });

                var studysetRef = _firebaseService._db.Collection("studysets").Document();

                var studysetData = new Dictionary<string, object>
                {
                    { "studyset_name", request.StudySetName ?? "Untitled Study Set" },
                    { "user_id", request.UserId ?? "test-user-id" },
                    { "studyset_color", request.StudySetColor ?? "blue" },
                    { "total_flashcards", 0 },
                    { "visibility", request.Visibility ?? "private" }
                };

                await studysetRef.SetAsync(studysetData);

                _memoryCache.Remove("studysets");
                return Ok(new { Message = "Study Set Created" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Error creating study set", ex);
            }

        }

        [HttpPost("edit")]
        public async Task<IActionResult> EditStudySet([FromBody] Studyset request)
        {
            try
            {
               
                var studysetRef = _firebaseService._db.Collection("studysets").Document(request.Id);

                var studysetData = new Dictionary<string, object>
                {
                    {"studyset_name", request.StudySetName },
                    {"studyset_color", request.StudySetColor},
                    {"user_id", "test-user-id" }, 
                    {"visibility", request.Visibility ?? "private"}
                };
                await studysetRef.UpdateAsync(studysetData);
                return Ok(new { Message = "Study Set Edited" });

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Editing Stuydset: {ex.Message}");
                return BadRequest(ex.Message);
            }

        }

        // Delete Studyset
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteStudyset([FromBody] Studyset request)
        {
            try
            {
                var studysetRef = _firebaseService._db.Collection("studysets").Document(request.Id);

                await studysetRef.DeleteAsync();
                return Ok(new { Message = "Study Set Deleted" });

            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting study set: {ex.Message}");
            }
        }

        // Fetch all Studysets based on User Id
        //[HttpGet("all/{userId}")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllStudysets()
        {
            string userId = "test-user-id";
            List<Studyset> studysets;
            try
            {
                    studysets = new();

                    var studysetsRef = _firebaseService._db.Collection("studysets");
                    var query = studysetsRef.WhereEqualTo("user_id", userId);
                    var snapshot = await query.GetSnapshotAsync();
                    foreach (var document in snapshot.Documents)
                    {
                        document.TryGetValue("total_flashcards", out int count);
                        studysets.Add(new Studyset
                        {
                            Id = document.Id,
                            StudySetColor = document.GetValue<string>("studyset_color"),
                            StudySetName = document.GetValue<string>("studyset_name"),
                            FlashcardCount = count,
                            Visibility = document.GetValue<string>("visibility")
                        });

                    }

                  

                
                return Ok(studysets);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching study sets: {ex.Message}");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCurrentStudyset(string id)
        {
            try
            {
                var studysetRef = _firebaseService._db.Collection("studysets").Document(id);

                var snapshot = await studysetRef.GetSnapshotAsync();

                if (!snapshot.Exists)
                {
                    return NotFound($"Studyset with ID :{id} not Found");
                }

                var studyset = new Studyset
                {
                    Id = snapshot.Id,
                    StudySetName = snapshot.GetValue<string>("studyset_name"),
                    StudySetColor = snapshot.GetValue<string>("studyset_color"),
                    UserId = snapshot.GetValue<string>("user_id"),
                    Visibility = snapshot.GetValue<string>("visibility")
                };

                return Ok(studyset);
            }catch (Exception ex)
            {
                throw new Exception("Error Fetching current studyset");
            }
           
        }
    }
}
