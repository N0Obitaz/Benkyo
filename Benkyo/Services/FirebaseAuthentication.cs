using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;

namespace Benkyo.Services
{
    public class FirebaseAuthentication
    {

        public FirebaseAuthConfig Config { get;  }
        

        public FirebaseAuthentication(IConfiguration firebaseCredential)
        {

            try
            {
                var apiKey = firebaseCredential["Firebase:ApiKey"];
                var authDomain = firebaseCredential["Firebase:AuthDomain"];
                Config = new FirebaseAuthConfig
                {
                    ApiKey = apiKey,
                    AuthDomain = authDomain,
                    Providers = new FirebaseAuthProvider[]
                    {
                        new EmailProvider()
                    },
                    UserRepository = new FileUserRepository("benkyo")
                   
                };
            } catch(Exception ex)
            
            {
                throw new Exception("An error occurred while initializing Firebase Authentication.", ex);
            }

        }
    }
}
