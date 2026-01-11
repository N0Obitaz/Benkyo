using Benkyo.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Benkyo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonController : ControllerBase
    {
        private readonly FirebaseService _firebaseService;

        public LessonController(FirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        [HttpPost("lesson/create")]
        private async Task<IActionResult> CreateLesson([FromBody] Lesson lessonRequest)
        {
            try
            {
                var existing = await _firebaseService._db.Collection("lessons").GetSnapshotAsync();
                
                if (!(existing == null)) return BadRequest(new { Message = "That lesson already exists" });
                {
                    var lessonRef = _firebaseService._db.Collection("lessons").Document();
                    var lessonData = new Dictionary<string, object>
                    {
                        { "lessonName", lessonRequest.LessonName ?? "Untitled Lesson" },
                        { "flashcards", lessonRequest.Flashcards ?? new List<Flashcard>() }
                    };
                    await lessonRef.SetAsync(lessonData);

                    return Ok(new { Message = "Lesson Created" });
                }


            }
            catch (Exception ex)
            {
                throw new Exception("Error creating lesson", ex);
            }
        }
    }
}
