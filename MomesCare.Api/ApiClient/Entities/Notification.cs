

namespace MomesCare.Api.ApiClient.Entitis
{
    
    public class Notification
    {
        public string title { get; set; }
        public string body { get; set; }
        public string topic { get; set; }
        public string date { get; set; }
        public string tag { get; set; }
     
        public int type { get; set; }
        public int status { get; set; }

        //public string Token { get; set; } = "";


    }
}
