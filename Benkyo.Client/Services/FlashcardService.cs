using System.Net.Http.Json;
using System.Text.Json;
using Shared.Models;

namespace Benkyo.Client.Services
{
    public class FlashcardService
    {
        private readonly HttpClient _httpClient;

        private HttpClient ht = new HttpClient { BaseAddress = new Uri("https://localhost:7218") };
}

        public FlashcardService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Flashcard>> GetStudysetFlashcardAsync(string studysetId)
        {
            try
            {

                using var ht = new HttpClient { BaseAddress = new Uri( "https://localhost:7218" ) };

                var response = await ht.GetStringAsync("api/flashcard/all");

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var flashcards = JsonSerializer.Deserialize<List<Flashcard>>(response, options);

                if (flashcards != null) return flashcards;

               
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<Flashcard>();
        }
        public async Task<bool> CreateFlashcardAsync(Flashcard flashcard)
        {
            var response = await _httpClient.PostAsJsonAsync("api/flashcard/create", flashcard);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EditFlashcardAsync(Flashcard flashcard)
        {
            var response = await _httpClient.PostAsJsonAsync("api/flashcard/create", flashcard);

                return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteFlashcardAsync(Flashcard flashcard)
        {
            var response = await _httpClient.PostAsJsonAsync("api/flashcard/create", flashcard);

            return response.IsSuccessStatusCode;
        }

        public async Task<int> CountFlashcardAsync(string studentId)
        {
            var response = await                                                                                                                  
        }
    }
}
