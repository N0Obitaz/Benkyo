using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Shared.Models;

namespace Benkyo.Client.Services
{
    public class PublicStudysetService
    {
        private readonly HttpClient _ht = new HttpClient { BaseAddress = new Uri("localhost://7218") };

        private readonly IMemoryCache _memoryCache;
        public PublicStudysetService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public async Task<Studyset> GetAllStudysetsPerVisibility ()
        {
        
            try
            {
                var response = await _ht.GetStringAsync($"api/publicstudyset/all");


                if (response != null)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var studysets = JsonSerializer.Deserialize<List<Studyset>>(response, options);


                    return studysets ?? new List<Studyset>();
                }
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
        }
    }
}
