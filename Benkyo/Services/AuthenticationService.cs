using Shared.Models;

namespace Benkyo.Services
{
    public class AuthenticationService
    {

        public async Task AuthenticateUser(User user)
        {

            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error authenticating user: {ex.Message}");)
            }
        }

    }
}
