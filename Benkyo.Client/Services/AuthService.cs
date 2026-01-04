using System.Net.Http.Json;
using System.Text.Json;
using Shared.Models;
namespace Benkyo.Client.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }

        public async Task<LoginModel> LoginAsync(string email, string password)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/login", new LoginModel {  Email = email, Password = password});

                var jsonContent = await response.Content.ReadAsStringAsync();

                var loginResponse = JsonSerializer.Deserialize<LoginModel>(jsonContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if(response.IsSuccessStatusCode && loginResponse != null)
                {
                    Console.WriteLine("True Here");
                    return loginResponse;
                }
                return new LoginModel();

            
            }catch(Exception ex)
            {
                throw new Exception("An error occurred during login.", ex);
            }
        }
    }
}
