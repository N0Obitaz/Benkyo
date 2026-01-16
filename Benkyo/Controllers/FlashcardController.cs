using Benkyo.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Benkyo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlashcardController : ControllerBase
    {

        private readonly FirebaseService _firebaseService;

        public FlashcardController(FirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreateFlashcard([FromBody] Flashcard flashcardRequest)
        {
            try
            {

                // Doesn't need to check for existing flashcards since they can be duplicated in different lessons

                var flashcardRef = _firebaseService._db.Collection("flashcards").Document();

                var flashCardData = new Dictionary<string, object>
                {
                    { "Question", flashcardRequest.Question ?? "No Question" },
                    { "Answer", flashcardRequest.Answer ?? "No Answer" }
                };
                    await flashcardRef.SetAsync(flashCardData);
                    return Ok(new { Message = "Flashcard Created" });
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating flashcard", ex);
            }
        }

        [HttpPost("edit")]
        public async Task<IActionResult> EditFlashcard([FromBody] Flashcard flashcardRequest)
        {
            try
            {
                if (flashcardRequest != null)
                {
                    var flashcardRef = _firebaseService._db.Collection("flashcards").Document(flashcardRequest.Id);

                    var flashcardData = new Dictionary<string, object>
                    {

                         {"question", flashcardRequest.Question! },
                         {"answer", flashcardRequest.Answer! }


                    };
                    await flashcardRef.UpdateAsync(flashcardData);

                    return Ok(new { Message = "Flashcard updated!" });
                }
               
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
