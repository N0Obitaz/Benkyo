using System.Net.Http.Json;
using Shared.Models;

namespace Benkyo.Client.Services
{
    public class LessonService
    {
        private readonly HttpClient _httpClient;

        public LessonService(HttpClient httpClient)         
        {
            _httpClient = httpClient;
        }

        public async Task<bool> CreateLessonAsync(Lesson lesson)
        {
            var response = await _httpClient.PostAsJsonAsync("api/lesson/create", lesson);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EditLessonAsync(Lesson lesson) 
        {
            var response = await _httpClient.PostAsJsonAsync("api/lesson/edit", lesson);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteLessonAsync(string lessonId)
        {
            var response = await _httpClient.PostAsJsonAsync("api/lesson/delete", new { Id = lessonId });
            return response.IsSuccessStatusCode;
        }
    }
}
