using System.Security.Claims;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Shared.Models;

namespace Benkyo.Services
{
    public class AuthenticationService
    {
        private readonly HttpContext _http;

        public AuthenticationService(IHttpContextAccessor httpContextAccessor)
        {
            _http = httpContextAccessor.HttpContext!;
        }
        public async Task AuthenticateUser(User user)
        {

            try
            {
                if (user is null) return;
                FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(user.Token);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Uid!),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim(ClaimTypes.Role, user.Role!),
                    new Claim(ClaimTypes.GivenName, user.Name!)
                };

                // create new claims identity
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                };

                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);


                await _http.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProperties);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error authenticating user: {ex.Message}");
            }
        }

    }
}
