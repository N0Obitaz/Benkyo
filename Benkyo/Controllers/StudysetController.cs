using Benkyo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Benkyo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudysetController : ControllerBase
    {
        private readonly FirebaseService _firebaseService;


        public StudysetController(FirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }
        

        [HttpPost("create")]
        public async Task<IActionResult> CreateStudySet([FromBody] Studyset request)
        {
            Console.WriteLine("You're here na");
            
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
                    { "Lessons", request.Lessons ?? new List<Lesson>() }
                };

                await studysetRef.SetAsync(studysetData);
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
                Console.WriteLine(request.StudySetName);
                Console.WriteLine(request.StudySetColor);
                Console.WriteLine(request.Id);

                var studysetRef = _firebaseService._db.Collection("studysets").Document(request.Id);

                var studysetData = new Dictionary<string, object>
                {
                    {"studyset_name", request.StudySetName },
                    {"studyset_color", request.StudySetColor},
                    {"user_id", "test-user-id" }
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
            Console.WriteLine("Youre here na 1");

           List<Studyset> studysets = new List<Studyset>();
            try
            {
                var studysetsRef = _firebaseService._db.Collection("studysets");
                var query = studysetsRef.WhereEqualTo("user_id", userId);
                var snapshot = await query.GetSnapshotAsync();
                foreach (var document in snapshot.Documents)
                {
                    studysets.Add(new Studyset
                    {
                        Id = document.Id,
                        StudySetColor = document.GetValue<string>("studyset_color"),
                        StudySetName = document.GetValue<string>("studyset_name")
                    });
                   
                }
                return Ok(studysets);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching study sets: {ex.Message}");
            }
        }
    }
}
