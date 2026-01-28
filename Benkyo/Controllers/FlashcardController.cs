using System.Numerics;
using System.Runtime.InteropServices.ObjectiveC;
using Benkyo.Services;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Shared.Models;

namespace Benkyo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlashcardController : ControllerBase
    {

        private readonly FirebaseService _firebaseService;
        private readonly IMemoryCache _memoryCache;



        public FlashcardController(FirebaseService firebaseService, IMemoryCache memoryCache)
        {
            _firebaseService = firebaseService;
            _memoryCache = memoryCache;
        }

        [HttpGet("all{id}")]
        public async Task<IActionResult> FetchAllFlashcard(string id)
        {
          
            try
            {
                List<Flashcard> flashcards;

                flashcards = _memoryCache.Get<List<Flashcard>>(id);
                if(flashcards is null)
                {
                    flashcards = new List<Flashcard>();
                    var flashcardRef = _firebaseService._db.Collection("flashcards");
                    var query = flashcardRef.WhereEqualTo("studyset_id", id);
                    var snapshot = await query.GetSnapshotAsync();

                    foreach (var document in snapshot.Documents)
                    {
                        flashcards.Add(new Flashcard
                        {
                            Id = document.Id,
                            Question = document.GetValue<string>("question"),
                            Answer = document.GetValue<string>("answer"),
                            Tag = document.GetValue<string>("tag")
                        });
                    }
                    Console.WriteLine("Fetched New Flashcards");
                    _memoryCache.Set(id, flashcards, TimeSpan.FromMinutes(5));


                }

                return Ok(flashcards);

            } catch (Exception ex)

            {
                Console.WriteLine(ex.Message);
                return BadRequest(new { error = "Error:" });
            }
        }

        private async Task UpdateTotalFlashcard(string studysetId, string operation)
        {
            try
            {
                var studysetRef = _firebaseService._db.Collection("studysets").Document(studysetId);

                var snapshot = await studysetRef.GetSnapshotAsync();

                int total = snapshot.GetValue<int>("total_flashcards");

                int newTotalFlashcards = 0;
                    
               
                if(operation == "add")
                {
                    total++;
                    newTotalFlashcards += total;
                }



                var flashcardData = new Dictionary<string, object>
                {
                    {"total_flashcards",  newTotalFlashcards}
                };

                await studysetRef.UpdateAsync(flashcardData);
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
          
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreateFlashcard([FromBody] Flashcard flashcardRequest)
        {
            try
            {
                if (flashcardRequest.StudysetId == null) return BadRequest( new {Message = "Studyset Id reference not fount"});

                // Doesn't need to check for existing flashcards since they can be duplicated in different lessons

                var flashcardRef = _firebaseService._db.Collection("flashcards").Document();

                var flashCardData = new Dictionary<string, object>
                {
                    { "question", flashcardRequest.Question ?? "No Question" },
                    { "answer", flashcardRequest.Answer ?? "No Answer" },
                    { "studyset_id", flashcardRequest.StudysetId },
                    { "tag", flashcardRequest.Tag ?? "No Tag"},
                };
                // update 
                await flashcardRef.SetAsync(flashCardData);

                _memoryCache.Remove(flashcardRequest.StudysetId);
                _memoryCache.Remove("studysets");

                await UpdateTotalFlashcard(flashcardRequest.StudysetId, "add");

               // Update TotalFlashcards on based on studyset

                
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




            } catch (Exception ex)
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
        [HttpGet("count/{id}")]
        public async Task<IActionResult> CountFlashcards(string id)
        {

            Console.WriteLine("You're here na");
            var flashcardRef = _firebaseService._db.Collection("flashcards");

            var query = flashcardRef.WhereEqualTo("studyset_id", id);

            AggregateQuery countQuery = query.Count();
            var snapshot = await countQuery.GetSnapshotAsync();

            return Ok(new { totalCards = snapshot.Count ?? 0 });


        }

    }
}
