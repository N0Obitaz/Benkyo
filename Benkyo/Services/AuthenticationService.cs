using FirebaseAdmin.Auth;
using Shared.Models;

namespace Benkyo.Services
{
    public class AuthenticationService
    {

        public async Task AuthenticateUser(User user)
        {

            try
            {
                FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(user.Token);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error authenticating user: {ex.Message}");
            }
        }

    }
}
