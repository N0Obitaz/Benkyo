using System.Net.Http.Json;
using Shared.Models;

namespace Benkyo.Client.Services
{
    public class FlashcardService
    {
        private readonly HttpClient _httpClient;

        public FlashcardService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> CreateFlashcardAsync(Flashcard flashcard)
        {
            var response = await _httpClient.PostAsJsonAsync("api/flashcard/create", flashcard);

            return response.IsSuccessStatusCode;
        }
    }
}
