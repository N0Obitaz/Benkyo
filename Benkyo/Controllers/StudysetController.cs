using Benkyo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Benkyo.Controllers
{
    [Authorize]
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
        private async Task<IActionResult> CreateStudySet([FromBody] Studyset request)
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
                    { "StudySetName", request.StudySetName ?? "Untitled Study Set" },
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
        private async Task<IActionResult> EditStudySet([FromBody] Studyset request)
        {
            try
            {

                var studysetRef = await _firebaseService._db.Collection("studysets").Document(request.Id ?? "").GetSnapshotAsync();

                var studysetData = new Dictionary<string, object>
                {
                    { "StudySetName", request.StudySetName }
                };
                await studysetRef.Reference.UpdateAsync(studysetData);
                return Ok(new { Message = "Study Set Edited" });

            }
            catch (Exception ex)
            {
                throw new Exception($"Error editing study set{ex.Message}");
            }

        }

        // Delete Studyset
        [HttpPost("delete")]
        private async Task<IActionResult> DeleteStudyset([FromBody] Studyset request)
        {
            try
            {
                var studysetRef = _firebaseService._db.Collection("studysets").Document(request.Id ?? "");

                await studysetRef.DeleteAsync();
                return Ok(new { Message = "Study Set Deleted" });

            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting study set: {ex.Message}");
            }
        }

        // Fetch all Studysets based on User Id
        [HttpGet("all/{userId}")]
        private async Task<IActionResult> GetAllStudysets(string userId)
        {
            try
            {
                var studysetsRef = _firebaseService._db.Collection("studysets");
                var query = studysetsRef.WhereEqualTo("UserId", userId);
                var snapshot = await query.GetSnapshotAsync();
                var studysets = new List<Studyset>();
                foreach (var document in snapshot.Documents)
                {
                    var studyset = document.ConvertTo<Studyset>();
                    studyset.Id = document.Id;
                    studysets.Add(studyset);
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
