using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Shared.Models;

namespace Benkyo.Client.Services
{
   
    public class StudysetService
    {

        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _memoryCache;

        //REMOVE THIS LATER: temporary userID
        private readonly string userId = "test-user-id";

        public StudysetService(HttpClient httpClient, IMemoryCache memoryCache)
        {
            _httpClient = httpClient;
            _memoryCache = memoryCache;
        }
        // Implement study set related methods here

        //will need userId to get studysets for specific user I'll change it later
        public async Task<List<Studyset>> GetUserStudySetAsync()
        {
            
            
            if(_memoryCache.TryGetValue(userId, out List<Studyset>? result)) {
                return result;
            }

            try
            {
                using var ht = new HttpClient { BaseAddress = new Uri("https://localhost:7218") };
                //this will be changed as there are user catered studysets

                var response = await ht.GetStringAsync("api/studyset/all");

                if(response != null)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var studysets = JsonSerializer.Deserialize<List<Studyset>>(response, options);

                    _memoryCache.Set(userId, studysets, TimeSpan.FromMinutes(5));
                    return studysets ?? new List<Studyset>();
                }
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

                if (studyset is not null)
                {
                    Console.WriteLine("Current Studyset Successully fetched");
                    return studyset;
                }

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
