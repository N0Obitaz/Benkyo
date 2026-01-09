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
        private readonly FirebaseAuthClient client;

        public AuthController(FirebaseService firebaseService,FirebaseAuthentication firebaseAuth)
        {
            _firebaseAuth = firebaseAuth;
            _firebaseService = firebaseService;
            client = new FirebaseAuthClient(_firebaseAuth.Config);
        }

        [HttpPost("login")] 
        public async Task<IActionResult> Login([FromBody] Shared.Models.User request)
        {
            try
            {
                var existing = await _firebaseService.GetUserByEmailAsync(request.Email);

                if(existing == null)
                {
                    return BadRequest(new { error = "User Does not Exist." });
                }
               
                var user = await client.SignInWithEmailAndPasswordAsync(request.Email!, request.Password!);

                // Implement login logic using _firebaseService
                // For example, verify user credentials and generate a token
                // Placeholder response
                var response = new Shared.Models.User
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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Shared.Models.User request)
        {
            try
            {
                var existing = await _firebaseService.GetUserByEmailAsync(request.Email);
                if(existing != null)
                {
                    return BadRequest(new { Error = "User Already Exists." });
                }

                var userRecord = await _firebaseService.CreateUserAsync(request.Email!, request.Password!);


                var userRef = _firebaseService._db.Collection("users").Document(userRecord.Uid);

                var userData = new Dictionary<string, object>
                {
                    {"email", request.Email! },
                    { "createdAt", Timestamp.GetCurrentTimestamp() },
                    {"uid", userRecord.Uid! }
                };

                await userRef.SetAsync(userData);

                return Ok(new Shared.Models.User{ Email = userRecord.Email, Uid = userRecord.Uid});

            } catch (Exception ex)
            {
                throw new Exception("An error occurred during registration.", ex);
            }
        }



    }
}
