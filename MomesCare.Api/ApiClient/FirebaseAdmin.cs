

using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Firebase.Database;
using Microsoft.Extensions.Options;
using MomesCare.Api.ApiClient.Entitis;

namespace MomesCare.Api.ApiClient
{
    public class FirebaseAdmin
    {
        private readonly  FirebaseApp   firebaseApp;
        private readonly IOptions<FirebaseConfig> options;
        private readonly FirebaseClient database;

        public FirebaseAdmin(IOptions<FirebaseConfig> options)
        {
            this.options = options;

            firebaseApp = FirebaseApp.Create(new AppOptions() {
         
                Credential = GoogleCredential.FromFile("../serviceAccountKey.json"),
                //ServiceAccountId= "my-client-id@my-project-id.iam.gserviceaccount.com",
            });

            

            database =new Firebase.Database.FirebaseClient(options.Value.DatabaseURL);
            
        }





    public static class FirebaseRefs
    {
        //public static CollectionReference Spots => db.Collection("Spots");
        //public static CollectionReference Bookings => db.Collection("Bookings");
        //public static CollectionReference ParkingGroups => db.Collection("ParkingGroups");
        //public static CollectionReference Users => db.Collection("Users");
        //public static CollectionReference BookingTemp => db.Collection("BookingTemp");
        //public static CollectionReference Notifications => db.Collection("Notifications");
    }
}
}
