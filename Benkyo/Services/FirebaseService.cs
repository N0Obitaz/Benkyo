using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;

namespace Benkyo.Services
{
    public class FirebaseService
    {

        private readonly FirestoreDb _db;
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

    }
}
