using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Shared.Models;

namespace Benkyo.Client.Services
{
    public class FlashcardService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _memoryCache;

        private HttpClient ht = new HttpClient { BaseAddress = new Uri("https://localhost:7218") };


        public FlashcardService(HttpClient httpClient, IMemoryCache memoryCache)
        {
            _httpClient = httpClient;
            _memoryCache = memoryCache;
        }

        public async Task<List<Flashcard>> GetStudysetFlashcardAsync(string studysetId)
        {
            if(_memoryCache.TryGetValue(studysetId, out List<Flashcard>? result))
            {
                Console.WriteLine("Fetched Cached Flashcard");
                return result;
            }

            try
            {

                var response = await ht.GetStringAsync($"api/flashcard/all{studysetId}");

                if(response != null)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var flashcards = JsonSerializer.Deserialize<List<Flashcard>>(response, options);

                    _memoryCache.Set(studysetId, flashcards, TimeSpan.FromMinutes(5));
                    Console.WriteLine("Fetched New set of Flashcards");

                    return flashcards ?? new List<Flashcard>();
                }
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

        public async Task<int> CountFlashcardAsync(string studysetId)
        {
            try
            {
                Console.WriteLine($"Id is  " + studysetId);
                var response = await ht.GetStringAsync($"api/flashcard/count{studysetId}");

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var converted = JsonSerializer.Deserialize<int>(response, options);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            

            return 0;
        }
    }
}
