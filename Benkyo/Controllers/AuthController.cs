using Benkyo.Services;
using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;

namespace Benkyo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly FirebaseService _firebaseService;
        private readonly FirebaseAuthentication _firebaseAuth;

        public AuthController(FirebaseService firebaseService,FirebaseAuthentication firebaseAuth)
        {
            _firebaseAuth = firebaseAuth;
            _firebaseService = firebaseService;
        }

        [HttpPost("login")] 
        public async Task<IActionResult> Login([FromBody] LoginModel request)
        {
            try
            {

                var client = new FirebaseAuthClient(_firebaseAuth.Config);

                var existing = await _firebaseService.GetUserByEmailAsync(request.Email);

                if(existing == null)
                {
                    return BadRequest(new { error = "User Does not Exist." });
                }
                Console.WriteLine("User Exists"); 
                var user = await client.SignInWithEmailAndPasswordAsync(request.Email!, request.Password!);

                // Implement login logic using _firebaseService
                // For example, verify user credentials and generate a token
                // Placeholder response
                var response = new LoginModel
                {
                    Token = "sample_token",
                    Expiration = DateTime.UtcNow.AddHours(1),
                    Email = request.Email
                };
                return Ok(new {Message = "You're Here"});

            } catch( Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("An error occurred during login.", ex);
                
               
            }
           
        }

    }
}
