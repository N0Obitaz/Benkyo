using System.Net.Http.Json;
using System.Text.Json;
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

        //will need userId to get studysets for specific user I'll change it later
        public async Task<List<Studyset>> GetUserStudySetAsync()
        {
            try
            {
                using var ht = new HttpClient { BaseAddress = new Uri("https://localhost:7218") };
                //this will be changed as there are user catered studysets

                var response = await ht.GetStringAsync("api/studyset/all");

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var studysets = JsonSerializer.Deserialize<List<Studyset>>(response, options);
             
                if (studysets is not null) return studysets;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
          return new List<Studyset>();
        }

        public async Task<Studyset> GetCurrentStudysetAsync(string id)
        {
            try
            {
                using var ht = new HttpClient { BaseAddress = new Uri("https://localhost:7218") };

                var response = await ht.GetStringAsync($"api/studyset/{id}");

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var studyset = JsonSerializer.Deserialize<Studyset>(response, options);

                if (studyset is not null) return studyset;
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                
            }
            return new Studyset();
        }

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
