using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;

namespace Benkyo.Services
{
    public class FirebaseService
    {

        public readonly FirestoreDb _db;
        private readonly FirebaseApp _firebaseApp;

        public FirebaseService(FirestoreDb db, FirebaseApp firebaseApp)
        {
            _db = db;
            _firebaseApp = firebaseApp;
        }

        public async Task<UserRecord> GetUserByEmailAsync(string email)
        {
            try
            {
                return await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(email);

            } catch
            {
                return null;
            }
        }
        public async Task<UserRecord> CreateUserAsync(string email, string password)
        {
            try
            {
                return await FirebaseAuth.DefaultInstance.CreateUserAsync(new UserRecordArgs
                {
                    Email = email,
                    Password = password
                });
            } catch
            {
                return null;
            }
            
        }
    }
}
