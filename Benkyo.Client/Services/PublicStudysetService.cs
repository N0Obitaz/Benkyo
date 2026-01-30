namespace Benkyo.Client.Services
{
    public class PublicStudysetService
    {
        private readonly HttpClient ht = new HttpClient { BaseAddress = new Uri("localhost://7218") };
    }
}
