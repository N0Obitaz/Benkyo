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

        public async Task<User> LoginAsync(string email, string password)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/login", new User {  Email = email, Password = password});

                var jsonContent = await response.Content.ReadAsStringAsync();

                var loginResponse = JsonSerializer.Deserialize<User>(jsonContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if(response.IsSuccessStatusCode && loginResponse != null)
                {
                    Console.WriteLine("True Here");
                    return loginResponse;
                }
                return new User();

            
            }catch(Exception ex)
            {
                throw new Exception("An error occurred during login.", ex);
            }
        }
        public async Task<User> RegisterAsync(User user)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", user);

            var jsonContent = await response.Content.ReadAsStringAsync();

            var registerResponse = JsonSerializer.Deserialize<User>(jsonContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if(response.IsSuccessStatusCode && registerResponse != null)
            {
                return registerResponse;
            }
            return new User();
        }
    }
}
