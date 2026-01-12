using System.Net.Http.Json;
using Shared.Models;

namespace Benkyo.Client.Services
{
   
    public class StudysetService
    {

        private readonly HttpClient _httpClient;

        public StudysetService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        // Implement study set related methods here
        public async Task<bool> CreateStudySetAsync(Studyset studyset)
        {
            var response = await _httpClient.PostAsJsonAsync("api/studyset/create", studyset);


            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine("Study Set Created Successfully");
            }
            return response.IsSuccessStatusCode;
            

                
        }

        public async Task<bool> EditStudySetAsync(Studyset studyset)
        {
            var response = await _httpClient.PostAsJsonAsync("api/studyset/edit", studyset);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteStudySetAsync(string studysetId)
        {
            var response = await _httpClient.PostAsJsonAsync("api/studyset/delete", new { Id = studysetId });
            return response.IsSuccessStatusCode;
        }

    }
}
