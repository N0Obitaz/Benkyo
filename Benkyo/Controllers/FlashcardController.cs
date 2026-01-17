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

        [HttpGet("all")]
        public async Task<IActionResult> FetchAllFlashcard([FromBody] Flashcard flashcardRequest)
        {
            string userId = "test-user-id";
            try
            {
                List<Flashcard> flashcards = new();
                var flashcardRef = _firebaseService._db.Collection("flashcards");
                var query = flashcardRef.WhereEqualTo("user_id", userId);
                var snapshot = await query.GetSnapshotAsync();

                foreach(var document in snapshot.Documents)
                {
                    flashcards.Add(new Flashcard
                    {
                        Id = document.Id,
                        Question = document.GetValue<string>("question"),
                        Answer = document.GetValue<string>("answer"),
                        Tag = document.GetValue<string>("tag")
                    });
                }
                return Ok(flashcards);

            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Error fetcing flashcards", ex);
            }
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
                
                    var flashcardRef = _firebaseService._db.Collection("flashcards").Document(flashcardRequest.Id);

                    var flashcardData = new Dictionary<string, object>
                    {

                         {"question", flashcardRequest.Question! },
                         {"answer", flashcardRequest.Answer! }


                    };
                    await flashcardRef.UpdateAsync(flashcardData);

                    return Ok(new { Message = "Flashcard updated!" });
           

          
               
            }catch(Exception ex)
            {

                throw new Exception("Error Deleting Flashcard", ex);
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteFlashcard([FromBody] Flashcard flashcardRequest)
        {
            try
            {
                
                    var flashcardRef = _firebaseService._db.Collection("flashcards").Document(flashcardRequest.Id);

                    await flashcardRef.DeleteAsync();

                    return Ok(new { Message = "Flashcard Deleted!" });
               

                   
            }
            catch (Exception ex)
            {

                throw new Exception("Error Deleting Flashcard", ex);
{

                };
            }
        }

    }
}
