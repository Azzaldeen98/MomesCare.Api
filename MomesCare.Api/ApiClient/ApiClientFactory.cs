using Firebase.Database;
using Microsoft.Extensions.Options;
using MomesCare.Api.ApiClient.Entitis;

namespace MomesCare.Api.ApiClient
{
    public interface IFirebaseClientFactory
    {
        FirebaseClient GetClient();

    }

    public class FirebaseClientFactory : IFirebaseClientFactory
    {

        private readonly FirebaseConfig _firebaseConfig;

        public FirebaseClientFactory(IOptions<FirebaseConfig> options)
        {
            _firebaseConfig = options.Value;
        }

        
        public FirebaseClient GetClient()
        {
            FirebaseClient firebaseClient = new FirebaseClient(_firebaseConfig.DatabaseURL);
            return firebaseClient;

        }
    }
}
