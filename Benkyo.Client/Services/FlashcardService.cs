namespace Benkyo.Client.Services
{
    public class FlashcardService
    {
        private readonly HttpClient _httpClient;

        public FlashcardService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
    }
}
