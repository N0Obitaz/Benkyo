using System.Security.Claims;
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
                if (user is null) return;
                FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(user.Token);

               

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error authenticating user: {ex.Message}");
            }
        }

    }
}
