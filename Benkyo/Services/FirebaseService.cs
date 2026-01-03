using Google.Cloud.Firestore;

namespace Benkyo.Services
{
    public class FirebaseService
    {

        private readonly FirestoreDb _db;

        public FirebaseService(FirestoreDb db)
        {
            _db = db;
        }

    }
}
